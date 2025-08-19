using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace BivliotecaAPI.Swagger
{
    public class ConvencionAgrupaPorVersion: IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var version = controller.ControllerType.Namespace?.Split('.').Last().ToLower();
            if (version is null)
            {
                return;
            }
            controller.ApiExplorer.GroupName = version;
           
        }
    }
}
