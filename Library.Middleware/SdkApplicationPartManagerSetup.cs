using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Library.Middleware;


// 這個類別負責「在 MVC 初始化完成後」，將 SDK 內的 Controllers 加到 ApplicationPartManager。
public class SdkApplicationPartManagerSetup : IConfigureOptions<MvcOptions>
{
    private readonly ApplicationPartManager _applicationPartManager;

    // ApplicationPartManager 是由 MVC 在 AddControllers() 時期註冊的
    public SdkApplicationPartManagerSetup(ApplicationPartManager applicationPartManager)
    {
        _applicationPartManager = applicationPartManager;
    }

    public void Configure(MvcOptions options)
    {
        // 取這個類型所在的 Assembly，假設整個 SDK 的 Controllers 都在這裡
        // 如果有另一個專門的 MarkerType，也可改為 typeof(MyMarkerTypeInSdk).Assembly
        var sdkAssembly = AppDomain.CurrentDomain.GetAssemblies()
            // 排除動態產生或沒有實際檔案路徑的組件，避免不必要的例外
            .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location));

        foreach (var assembly in sdkAssembly)
        {
            // 檢查是否已經加入，避免重複
            if (!_applicationPartManager.ApplicationParts.Any(p => p.Name == assembly.GetName().Name))
            {
                _applicationPartManager.ApplicationParts.Add(new AssemblyPart(assembly));
            }
        }
    }
}