# 鱼漂统计与上报

在 ACT 的 Matcha 设置中主动开启“启用鱼漂咬钩时间查询及钓鱼数据上报”后生效；默认关闭，与 Universalis 授权独立。服务为 https://api.ffxiv.cyou/fisher/ 。关闭设置会取消请求、清空待上传队列并撤销在线统计。不会补传开启前的记录。

C# 负责请求，悬浮窗只接收 `FishingStatistics` v1 日志事件。查询只发送已确认的钓场 PlaceName ID；上传使用独立 CBOR 对象，仅含钓场、鱼饵、竿时、钓鱼属性、技能状态和鱼获结果，不含角色标识、名称或原始包。`FishingDTO` 不再输出角色 ID；内部角色标识仅用于过滤和会话切换。

## 协议与边界

协议参考固定提交 [ffxiv-fishing-float 13baacb3](https://github.com/ffxiv-cyou/ffxiv-fishing-float/tree/13baacb3e14c2db848855f43d204decdcddc92dd)，独立实现查询校验及 CBOR 编码。`GET /fisher/fish-duration?spot_id=…&min_count=10` 使用 distributions，按鱼饵及撒饵状态筛选，不采用 merged。`POST /fisher/upload` 携带 `X-Client-Timestamp`，数组字段顺序参见 `FishingUpload.Create`。

已确认的钓场目录由 `utils/update-data.mjs` 从 FishingSpot 的 PlaceName 生成。0、950、缺失及目录外 ID 都不查询或上传。每次普通抛竿重新确认钓场；以小钓大只继承仍在进行的钓鱼会话。状态基线、属性、提钩方式或鱼获字段不完整时跳过上报，查询不受影响。

查询超时 5 秒、响应上限 2 MiB；同钓场合并请求，最多缓存 32 个钓场，10 分钟刷新、空结果 5 分钟、24 小时失效。失败至少冷却 60 秒并遵守 Retry-After。停止钓鱼后不重试查询。上传每 60 秒一次，最多 120 竿、10 分钟，只存内存；结果不明确的批次不重发，新记录与在途批次分离。登出、进程切换、卸载及撤销授权清空队列。

## 属性读取兼容性

通过只读进程句柄读取 PlayerState，不注入或调用客户端函数。获得力、鉴别力和装备鱼饵来自 PlayerState；当前/最大 GP 来自 ACT 公开角色列表，允许 GP 为 0。内存线程每 500 ms 采样，抛竿时固定最近 2 秒内的不可变快照。已有换饵、以小钓大或任务鱼饵事件优先于内存补全。

布局来源为 FFXIVClientStructs `25762ad04` 的 PlayerState。仅允许以下静态验证过的 exe SHA-256，其他构建返回未知，不能直接套用最新国际服偏移：

- CN：`7ba28760bc53cbbbf66b63105fea6c1bfe09cb6b1c6353fc8fb0ff03cc1a30f4`
- Global：`2d5457305db876f4bf68080f7703dd187375cd7e2fd607fa211b3b3c204e4b25`

两个文件均已验证静态地址签名唯一及属性索引读取；CN 的 PlayerSetup 处理验证了鱼饵写入。匹配不唯一、角色未加载、进程变化或读取失败均返回未知。钓鱼状态基线仅接入普通 `StatusEffectList`，随后结合 `ActorControl` 状态增减事件维护。不接入未经个人钓鱼场景确认的 `StatusEffectList2/3`；未取得完整状态基线时跳过上传。

## 验证

`dotnet run --project tests/FishingService` 使用人工数据、模拟 HTTP 和人工 PE，不连接真实服务。覆盖授权、迟到响应、未知钓场、缓存和冷却、批次隔离、零 GP、属性快照、包事件关联、拟饵、以小钓大、失败鱼获、签名歧义及 CBOR 契约。`node --test utils/update-data.test.mjs` 验证目录生成。

Windows 实机验收仍需执行，不能以静态分析或 Linux 核心测试代替：

1. 编译完整插件，分别在支持的 CN/Global 构建核对获得力、鉴别力、GP、鱼饵，覆盖换装、食物、职业切换和进程切换。
2. 验证默认关闭不请求，主动开启后普通抛竿、换饵、撒饵、拟饵、以小钓大及脱钩行为。
3. 模拟断网及撤销授权，确认计时连续、在线结果撤销，角色切换不遗留队列。
4. 真实上传仅在用户主动开启后检查，不向服务发送人工测试样本。
