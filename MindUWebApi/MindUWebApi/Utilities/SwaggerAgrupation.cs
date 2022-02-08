using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

namespace MindUWebApi.Utilities
{
    public class SwaggerAgrupation : IControllerModelConvention
    {
        public void Apply (ControllerModel controller)
        {
            var namespaceController = controller.ControllerType.Namespace; //Controllers.Vx
            var versionApi = namespaceController.Split('.').Last().ToLower();
            controller.ApiExplorer.GroupName = versionApi;
        }
    }
}
