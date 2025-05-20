import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { GameService, GameDto, GameStatus, gameStatusOptions } from '../../proxy/games';
import { GetGameListDto } from '../../proxy/games/dtos';
import { GameUploadService } from '../../proxy/controllers/game-upload.service';
import { PermissionService } from '@abp/ng.core';
import { GameRepositoryPermissions } from '../permissions/game-repository-permissions';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { environment } from '../../../environments/environment';

declare var bootstrap: any;
@Component({
  selector: 'app-game-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './game-list.component.html',
  styleUrl: './game-list.component.scss'
})
export class GameListComponent implements OnInit {
  // Game data
  games: GameDto[] = [];
  selectedGame: GameDto | null = null;
  totalCount = 0;
  private readonly API_BASE_URL = environment.apis.default.url;

  // Pagination
  pageSize = 10;
  currentPage = 1;
  totalPages = 1;

  // Filtering
  currentFilter = '';
  currentStatus: GameStatus | null = null;

  // Status handling
  selectedStatus: GameStatus | null = null;
  statusOptions = gameStatusOptions;
  GameStatus = GameStatus; // For template access

  // Modals
  statusChangeModal: any;
  deleteConfirmModal: any;

  // Permission flags
  hasManagePermission = false;
  hasEditPermission = false;
  hasDeletePermission = false;

  // File inputs
  @ViewChild('iconInput') iconInput!: ElementRef<HTMLInputElement>;
  @ViewChild('gamePackageInput') gamePackageInput!: ElementRef<HTMLInputElement>;

  constructor(
    private gameService: GameService,
    private gameUploadService: GameUploadService,
    private permissionService: PermissionService,
    private confirmationService: ConfirmationService,
    private toastService: ToasterService
  ) {}

  ngOnInit(): void {
    this.checkPermissions();
    this.getGameList();
  }

  ngAfterViewInit(): void {
    // Initialize modals
    this.initModals();
  }

  private initModals(): void {
    setTimeout(() => {
      const statusModalElement = document.getElementById('statusChangeModal');
      if (statusModalElement) {
        this.statusChangeModal = new bootstrap.Modal(statusModalElement);
      }

      const deleteModalElement = document.getElementById('deleteConfirmModal');
      if (deleteModalElement) {
        this.deleteConfirmModal = new bootstrap.Modal(deleteModalElement);
      }
    }, 0);
  }

  private checkPermissions(): void {
    this.hasManagePermission = this.permissionService.getGrantedPolicy(GameRepositoryPermissions.Games.Manage);
    this.hasEditPermission = this.permissionService.getGrantedPolicy(GameRepositoryPermissions.Games.Edit);
    this.hasDeletePermission = this.permissionService.getGrantedPolicy(GameRepositoryPermissions.Games.Delete);
  }

  // 修改 getGameList 方法，确保图标 URL 使用完整地址
  getGameList(): void {
    const input: GetGameListDto = {
      skipCount: (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
      filter: this.currentFilter || undefined,
      status: this.currentStatus !== null ? this.currentStatus : undefined
    };

    this.gameService.getList(input).subscribe(response => {
      // 处理图标 URL
      this.games = response.items.map(game => {
        // 如果 iconUrl 是相对路径，添加 API 基础 URL
        if (game.iconUrl && !game.iconUrl.startsWith('http')) {
          game.iconUrl = `${this.API_BASE_URL}${game.iconUrl.startsWith('/') ? '' : '/'}${game.iconUrl}`;
        }
        return game;
      });
      this.totalCount = response.totalCount;
      this.calculateTotalPages();
    });
  }

  calculateTotalPages(): void {
    this.totalPages = Math.ceil(this.totalCount / this.pageSize);
  }

  getPageArray(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  loadPage(page: number): void {
    if (page < 1 || page > this.totalPages) {
      return;
    }
    this.currentPage = page;
    this.getGameList();
  }

  filterGames(filterText: string): void {
    this.currentFilter = filterText;
    this.currentPage = 1; // Reset to first page
    this.getGameList();
  }

  filterByStatus(status: GameStatus | null): void {
    this.currentStatus = status;
    this.currentPage = 1; // Reset to first page
    this.getGameList();
  }

  onUploadGame(form: NgForm): void {
    if (form.invalid) {
      return;
    }

    const iconFile = this.iconInput.nativeElement.files?.[0];
    const gamePackageFile = this.gamePackageInput.nativeElement.files?.[0];

    if (!iconFile || !gamePackageFile) {
      this.toastService.error('Please select both an icon and a game package file');
      return;
    }

    this.gameUploadService.uploadGame(
      form.value.gameName,
      form.value.gameDescription || '',
      form.value.gameDeveloper,
      form.value.gameVersion,
      iconFile as unknown as any,
      gamePackageFile as unknown as any
    ).subscribe({
      next: (result) => {
        this.toastService.success(`Game "${result.name}" uploaded successfully and is pending review`, 'Success');
        // Reset form and close modal
        form.resetForm();
        this.iconInput.nativeElement.value = '';
        this.gamePackageInput.nativeElement.value = '';
        this.hideUploadModal();
        // Refresh game list
        this.getGameList();
      },
      error: (error) => {
        this.toastService.error(`Error uploading game: ${error.error?.message || 'Unknown error'}`, 'Error');
      }
    });
  }

  hideUploadModal(): void {
    const modalElement = document.getElementById('uploadGameModal');
    if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      if (modal) {
        modal.hide();
      }
    }
  }

  openStatusChangeModal(game: GameDto): void {
    this.selectedGame = game;
    this.selectedStatus = game.status;

    // 直接使用数据属性方式打开模态框
    const modalElement = document.getElementById('statusChangeModal');
    if (modalElement) {
      // 安全地检查bootstrap是否存在
      if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
        const modal = new bootstrap.Modal(modalElement);
        modal.show();
        this.statusChangeModal = modal;
      } else {
        // 回退方案：添加显示类
        modalElement.classList.add('show');
        modalElement.style.display = 'block';
        document.body.classList.add('modal-open');
        const backdrop = document.createElement('div');
        backdrop.className = 'modal-backdrop fade show';
        document.body.appendChild(backdrop);
      }
    }
  }

  setSelectedStatus(status: GameStatus): void {
    this.selectedStatus = status;
  }

  updateGameStatus(): void {
    if (!this.selectedGame || this.selectedStatus === null) {
      return;
    }

    this.gameService.changeStatus(this.selectedGame.id, {
      status: this.selectedStatus
    }).subscribe({
      next: (result) => {
        this.toastService.success(`Game status updated to ${GameStatus[result.status]}`, 'Success');
        // 更新列表中的游戏
        const index = this.games.findIndex(g => g.id === result.id);
        if (index !== -1) {
          this.games[index] = result;
        }

        // 隐藏模态框（添加回退方案）
        if (this.statusChangeModal) {
          this.statusChangeModal.hide();
        } else {
          const modalElement = document.getElementById('statusChangeModal');
          if (modalElement) {
            modalElement.classList.remove('show');
            modalElement.style.display = 'none';
            document.body.classList.remove('modal-open');
            const backdrop = document.querySelector('.modal-backdrop');
            if (backdrop) {
              backdrop.remove();
            }
          }
        }
      },
      error: (error) => {
        this.toastService.error(`Error updating status: ${error.error?.message || 'Unknown error'}`, 'Error');
      }
    });
  }

  confirmDelete(game: GameDto): void {
    this.selectedGame = game;

    // 直接初始化并打开模态框
    const modalElement = document.getElementById('deleteConfirmModal');
    if (modalElement) {
      // 安全地检查bootstrap是否存在
      if (typeof bootstrap !== 'undefined' && bootstrap.Modal) {
        const modal = new bootstrap.Modal(modalElement);
        modal.show();
        this.deleteConfirmModal = modal;
      } else {
        // 回退方案：添加显示类
        modalElement.classList.add('show');
        modalElement.style.display = 'block';
        document.body.classList.add('modal-open');
        const backdrop = document.createElement('div');
        backdrop.className = 'modal-backdrop fade show';
        document.body.appendChild(backdrop);
      }
    }
  }

  deleteGame(): void {
    if (!this.selectedGame) {
      return;
    }

    this.gameService.delete(this.selectedGame.id).subscribe({
      next: () => {
        this.toastService.success(`Game "${this.selectedGame!.name}" deleted successfully`, 'Success');
        // 隐藏模态框（添加回退方案）
        if (this.deleteConfirmModal) {
          this.deleteConfirmModal.hide();
        } else {
          const modalElement = document.getElementById('deleteConfirmModal');
          if (modalElement) {
            modalElement.classList.remove('show');
            modalElement.style.display = 'none';
            document.body.classList.remove('modal-open');
            const backdrop = document.querySelector('.modal-backdrop');
            if (backdrop) {
              backdrop.remove();
            }
          }
        }
        // 刷新列表
        this.getGameList();
      },
      error: (error) => {
        this.toastService.error(`Error deleting game: ${error.error?.message || 'Unknown error'}`, 'Error');
      }
    });
  }

// 修改 viewGame 方法，确保游戏访问 URL 使用完整地址
viewGame(game: GameDto): void {
  if (game.status !== GameStatus.Approved) {
    this.toastService.warn('Only approved games can be viewed');
    return;
  }

  this.gameService.getGameEntryUrl(game.id).subscribe({
    next: (url) => {
      console.log(url);

      // 如果返回的是相对 URL，添加 API 基础 URL
      if (url && !url.startsWith('http')) {
        url = `${this.API_BASE_URL}${url.startsWith('/') ? '' : '/'}${url}`;
      }
      window.open(url, '_blank');
    },
    error: (error) => {
      this.toastService.error(`Error retrieving game URL: ${error.error?.message || 'Unknown error'}`, 'Error');
    }
  });
}

  editGame(game: GameDto): void {
    // This could be implemented to navigate to an edit page or open an edit modal
    // For this example, we'll just show a toast that it's not implemented
    this.toastService.info('Edit functionality not implemented in this version');
  }
}
