# 食谱管理系统：Part A

目标框架为 .NET 10，测试框架为 xUnit。

仓库地址：https://github.com/hejiatao6666-hash/recipe-management-part-a-2026

## 当前进度

2026 年 11 月 5 日完成食谱目录：

- 使用 `Dictionary<int, Recipe>` 保存食谱。
- 构造时检查空集合参数、非正数 ID、空白标题和重复 ID。
- 添加、按 ID 查找和删除食谱，安全处理无效输入与缺失记录。
- 为五个指定集合建立私有字段，并提供对应的数量属性。
- 目录阶段的 18 项测试通过。

2026 年 11 月 6 日完成购物清单：

- 使用 `List<string>` 按原始顺序追加配料，保留字符串原文和重复项。
- 缺失食谱返回 0，没有配料的食谱不改变清单。
- 返回只读视图，避免调用方修改内部列表。
- 清空购物清单并更新数量，不改变食谱原有配料。
- 新增 9 项购物清单测试，目录与购物清单阶段共 27 项测试通过。
- 修复解决方案文件顶部多余空行导致的编译错误。

2026 年 11 月 7 日完成烹饪计划与移除历史：

- 使用 `LinkedList<int>` 按添加顺序保存计划，拒绝不存在和重复的食谱。
- 按 ID 移除计划中的食谱，仅成功移除时将 ID 压入 `Stack<int>`。
- 查看最近移除的 ID 不改变历史，恢复时将栈顶 ID 弹出并追加到计划末尾。
- 安全处理空栈；已删除或已重新加入计划的食谱无法恢复，但仍消耗这一条历史记录。
- 返回计划的只读副本，保护内部链表；目录中的计划内食谱不能被删除。
- 新增 15 项链表、栈与组件交互测试，连同原有历史示例测试，累计 43 项阶段测试通过。

指令队列的方法暂保留起始占位实现，原有的队列示例测试将在相应功能完成后通过。

## 实际开发安排

| 日期 | 工作内容 |
| --- | --- |
| 2026-11-05 | 起始项目、Git、食谱目录及目录测试 |
| 2026-11-06 | 购物清单及列表测试 |
| 2026-11-07 | 烹饪计划、移除历史及链表与栈测试 |
| 2026-11-08 | 指令队列、组件交互测试、全量验证与提交打包 |



## 项目结构

- `RecipeManagement.Application`：引用 Core。
- `RecipeManagement.Core`：模型、加载器、接口以及待完成的 `RecipeManager`。
- `RecipeManagement.Tests`：引用 Core 的 xUnit 测试。
- `data/recipes.json`：食谱数据。
- `Design.md`： Part B 模板。

## 编译、测试与运行

在本仓库根目录执行：

```powershell
dotnet build
dotnet test
dotnet run --project RecipeManagement.Application -- data/recipes.json
```

当前验证目录、购物清单、烹饪计划与移除历史阶段：

```powershell
dotnet test --filter "FullyQualifiedName!~InstructionsAreCompletedInFileOrder"
```



若 Windows 的默认 `dotnet` 提示没有 SDK，而 SDK 安装在用户目录，可使用：

```powershell
& "$env:USERPROFILE\.dotnet\dotnet.exe" build
```

`bin`、`obj`、测试结果和 IDE 临时文件不纳入 Git。

