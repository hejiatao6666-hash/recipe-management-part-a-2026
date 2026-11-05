# 食谱管理系统：Part A

基于课程提供的三项目起始代码开发，目标框架为 .NET 10，测试框架为 xUnit。

## 当前进度

2026 年 11 月 5 日完成食谱目录：

- 使用 `Dictionary<int, Recipe>` 保存食谱。
- 构造时检查空集合参数、非正数 ID、空白标题和重复 ID。
- 添加、按 ID 查找和删除食谱，安全处理无效输入与缺失记录。
- 为五个指定集合建立私有字段，并提供对应的数量属性。
- 目录阶段的 18 项测试通过。

购物清单、烹饪计划、移除历史和指令队列的方法暂保留起始占位实现。原有的队列和历史示例测试将在相应功能完成后通过。Part B 不在本次实现范围内。

## 实际开发安排

| 日期 | 工作内容 |
| --- | --- |
| 2026-11-05 | 起始项目、Git、食谱目录及目录测试 |
| 2026-11-06 | 购物清单及列表测试 |
| 2026-11-07 | 烹饪计划、移除历史及链表与栈测试 |
| 2026-11-08 | 指令队列、组件交互测试、全量验证与提交打包 |

后续日期是计划。Git 提交与远程同步只记录实际完成的工作。

## 项目结构

- `RecipeManagement.Application`：老师提供的控制台菜单，引用 Core。
- `RecipeManagement.Core`：老师提供的模型、加载器、接口以及待完成的 `RecipeManager`。
- `RecipeManagement.Tests`：引用 Core 的 xUnit 测试。
- `data/recipes.json`：老师提供的食谱数据。
- `Design.md`：老师提供的 Part B 模板。

## 编译、测试与运行

在本仓库根目录执行：

```powershell
dotnet build
dotnet test
dotnet run --project RecipeManagement.Application -- data/recipes.json
```

当前仅验证目录阶段：

```powershell
dotnet test --filter "FullyQualifiedName!~InstructionsAreCompletedInFileOrder&FullyQualifiedName!~RemovedRecipesAreRestoredLastInFirstOut"
```

完整的 `dotnet test` 当前还会运行两个未完成阶段的示例测试，不能将目录阶段通过等同于整份作业完成。

若 Windows 的默认 `dotnet` 提示没有 SDK，而 SDK 安装在用户目录，可使用：

```powershell
& "$env:USERPROFILE\.dotnet\dotnet.exe" build
```

`bin`、`obj`、测试结果和 IDE 临时文件不纳入 Git。

## AI 使用说明

本项目在用户确认授课教师允许使用 AI 的前提下，由 OpenAI Codex 协助解释要求、实现食谱目录、编写相关测试及排查开发环境问题。此处如实记录 AI 协助范围，不声明这些内容完全由学生独立编写。
