# 潜艇部件耐久事件

`SubmarineParts` 为 matcha-overlay 提供工房中的部件耐久快照，需要插件和 overlay 同时支持此事件。旧插件仍可显示潜艇信息，但耐久显示为未知。

## 数据来源

复用服务端 `ItemInfo` 和 `UpdateInventorySlot` 的 64 字节物品结构，以下偏移相对 IPC 载荷：

| 偏移 | 类型 | 含义 |
| --- | --- | --- |
| `0x08` | u16 | 容器编号 |
| `0x0a` | u16 | 槽位 |
| `0x10` | u32 | Item 表行号 |
| `0x22` | u16 | 耐久原始值 |

仅在部队工房上下文解释容器 `25004`：潜艇索引为 `slot / 5`，部件为 `slot % 5`，依次为船体、船尾、船首、舰桥；余数为 4 的槽位不解释。容器编号在其他房屋区域也有用途，不能全局当作潜艇容器。

耐久百分比按 `100 * condition / 30000` 计算，与 FFXIVClientStructs 的 `InventoryItem.GetConditionPercentage` 定义一致。零表示损坏，未收到信息表示未知，两者不能混用。此值不是 `0x24` 的精炼度。

## 事件契约

- `reset`：清除整个工房会话的状态。
- `index`：潜艇索引，范围 0–3；清除事件使用 -1。
- `parts`：四项数组，依次为船体、船尾、船首、舰桥。每项为 `{ itemId, condition }` 或 null。清除事件使用空数组。
- `itemId` 是 Item 表行号，不是 `SubmarineStatus` 中的 SubmarinePart 表行号。overlay 必须先转换并核对物品，再展示耐久。

每次物品更新发送该潜艇目前已知的四项快照。物品快照可能先于潜艇状态列表到达，接收方应暂存后再关联。登录、换区和退出时清除快照，不将耐久持久化；中途连接且没有收到物品数据时显示未知。

此事件不包含角色标识、潜艇名字或原始包内容。测试仅使用独立构造的数据。

## 验证

`dotnet run --project tests/SubmarineParts/SubmarineParts.csproj` 检查槽位映射、维修更新、零耐久、无效载荷和会话清理。该项目只链接解析器及必要依赖，不需要 ACT。完整插件仍需在 Windows / ACT 中构建并实机验证。
