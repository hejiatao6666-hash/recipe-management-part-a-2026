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

2026 年 11 月 8 日完成指令队列与综合验证：

- 使用 `Queue<string>` 按原始顺序加载指令，查看不移除，完成时只移除队首一条指令。
- 空队列返回 `null`；启动有效会话替换旧指令，失败的启动请求保留当前会话。
- 新增 12 项队列与完整流程测试，连同原有队列示例测试，全量 56 项测试通过。
- 完整流程测试覆盖目录、购物清单、计划、移除历史和指令队列的交互。
- 三项目编译通过，零警告、零错误；控制台加载 670 条食谱，购物清单、计划恢复和指令处理流程检查通过。

Part A 的全部要求已实现。Part B 方法保留起始占位实现，`Design.md` 是后续 Part B 的模板。

## 实际开发安排

| 日期 | 工作内容 |
| --- | --- |
| 2026-11-05 | 起始项目、Git、食谱目录及目录测试 |
| 2026-11-06 | 购物清单及列表测试 |
| 2026-11-07 | 烹饪计划、移除历史及链表与栈测试 |
| 2026-11-08 | 指令队列、组件交互测试、全量验证与提交打包 |



## 项目结构

- `RecipeManagement.Application`：引用 Core。
- `RecipeManagement.Core`：模型、加载器、接口以及 `RecipeManager` 的 Part A 实现。
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

若 Windows 的默认 `dotnet` 提示没有 SDK，而 SDK 安装在用户目录，可使用：

```powershell
& "$env:USERPROFILE\.dotnet\dotnet.exe" build
```

若本机受限终端运行测试时，测试宿主监控父进程提示访问被拒绝，可临时使用以下命令。此参数只用于本机运行环境，不筛选或跳过测试：

```powershell
dotnet test -p:_MSTestEnableParentProcessQuery=false
```

`bin`、`obj`、测试结果和 IDE 临时文件不纳入 Git。

## 提交版本与 Git 记录

最终提交版本使用 `part-a-final` 标签标识，仓库保留 2026 年 11 月 5 日至 8 日的实际开发提交。解压提交 ZIP 后，在项目根目录执行：

```powershell
git log --oneline --date=iso-strict --format="%h %ad %s"
git show part-a-final --stat
```

ZIP 包含隐藏的 `.git` 文件夹及源码、项目文件、测试和数据，不包含 `bin`、`obj`。GitHub 上的提交记录也可通过仓库首页的 Commits 入口查看。

