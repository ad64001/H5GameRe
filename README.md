# GameRepository 项目文档

## 一、项目概述
GameRepository 是一个基于 ABP 框架开发的分层启动解决方案，遵循领域驱动设计（DDD）实践，整合了所有基本的 ABP 模块。该项目包含一个 ASP.NET Core API 应用程序和一个 Angular 前端应用程序，用于管理游戏相关信息（如包和图标。

## 二、环境要求
### 后端
- [.NET 9.0+ SDK](https://dotnet.microsoft.com/download/dotnet)
- [Node v20.11+](https://nodejs.org/en)

### 前端
- Node.js 和 npm（Node Package Manager），通常随着 Node.js 一起安装

## 三、项目结构
本项目是一个分层的单体应用程序，包含以下部分：
- `GameRepository.DbMigrator`：一个控制台应用程序，用于应用数据库迁移并初始化数据。在开发和生产环境中都非常有用。
- `GameRepository.HttpApi.Host`：ASP.NET Core API 应用程序，用于向客户端暴露 API。
- `angular`：Angular 前端应用程序。

## 四、运行步骤

### 后端
#### 1. 数据库准备
检查 `GameRepository.HttpApi.Host` 和 `GameRepository.DbMigrator` 项目下的 `appsettings.json` 文件中的 `ConnectionStrings`，根据需要进行修改。
在首次运行应用程序之前，需要创建数据库。运行 `GameRepository.DbMigrator` 来创建初始数据库。如果后续解决方案中添加了新的数据库迁移，也需要再次运行该程序。
```bash
# 假设在项目根目录下，进入 DbMigrator 项目目录
cd H5GameRe/aspnet-core/src/GameRepository.DbMigrator
# 运行 DbMigrator
dotnet run
# 进入 HttpApi.Host 项目目录
cd H5GameRe/aspnet-core/src/GameRepository.HttpApi.Host
# 运行 API 应用程序
dotnet run
```
### 前端
#### 1. model安装
```bash
# 假设在项目根目录下，进入 DbMigrator 项目目录
cd H5GameRe\angular
# 运行 DbMigrator
npm install
npm start
```
## 五、完成启动部署
OK


