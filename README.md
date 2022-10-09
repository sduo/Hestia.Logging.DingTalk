# Hestia.Logging.DingTalk

[![](https://github.com/sduo/Hestia.Logging.DingTalk/actions/workflows/main.yml/badge.svg)](https://github.com/sduo/Hestia.Logging.DingTalk)
[![](https://img.shields.io/nuget/v/Hestia.Logging.DingTalk.svg)](https://www.nuget.org/packages/Hestia.Logging.DingTalk)

# 基础说明

1. 参照 [钉钉官方文档](https://open.dingtalk.com/document/robots/custom-robot-access) 创建钉钉机器人（```安全设置``` 使用 ```加签```）。
2. 在配置文件中的 ```Logging``` 节点下新增 ```DingTalk``` 节点，配置 ```Url``` 与 ```Secret```。

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.Hosting.Lifetime": "Information"
        },
        "DingTalk": {
          "IncludeScopes": true,
          "Url": "https://oapi.dingtalk.com/robot/send?access_token={AccessToken}",
          "Secret": "{Secret}",      
          "LogLevel": {
            "Default": "Warning"
          }
        }
      }
    }
    ```

3. 日志等级相关的配置可以参考 [微软官方文档](https://learn.microsoft.com/zh-cn/dotnet/core/extensions/logging)。

# 高级说明

## Markdown 输出配置

在 ```DingTalk``` 节点下，使用 ```Markdown``` 节点进行配置。

```json
{
  "Logging": {        
    "DingTalk": {
      "Markdown":{
        "Stack": false,
        "Image":{
          "Information":"![Information]({information.png})",
          "Warning":"![Warning]({warning.png})",
          "Error":"![Error]({error.png})"
        },
        "Icon":{
          "Information":"💚",
          "Warning":"💛",
          "Error":"❤️"
        }
      }          
    }
  }
}
```

* Stack：是否包含异常堆栈（布尔值，默认：是）
* Image：用指定的 Markdown 替换钉钉消息中首行的日志等级，如上例所示：用指定的图片替换对应的日志等级，来提高辨识度。
* Icon：用指定的字符替换钉钉聊天列表中新消息标题中的日志等级，如上例所示：用指定的 UTF8 图标来替换对应的日志等级，来提高辨识度。

## 自定义其他格式的消息

默认实现了 [Markdown](https://github.com/sduo/Hestia.Logging.DingTalk/blob/master/src/Formatters/MarkdownFormatter.cs) 消息，如果有特殊需要，参考 [钉钉官方文档](https://open.dingtalk.com/document/robots/custom-robot-access) 中的 ```消息类型及数据格式```，实现 [IFormatter](https://github.com/sduo/Hestia.Logging.DingTalk/blob/master/src/Formatters/IFormatter.cs) 接口即可。

# 依赖说明

* [Hestia.Logging.Abstractions](https://github.com/sduo/Hestia.Logging.Abstractions)：参考 [Logging.AzureAppServices](https://github.com/dotnet/aspnetcore/tree/main/src/Logging.AzureAppServices) 实现的日志提供程序。
* [Hestia.Security](https://github.com/sduo/Hestia.Security)：钉钉消息的 ```HMAC-SHA256``` 签名计算。

# 使用示例
* [Hestia.Logging.DingTalk.Samples](https://github.com/sduo/Hestia.Logging.DingTalk.Samples)
