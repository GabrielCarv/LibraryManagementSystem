using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.ViewComponents
{
    public class FormViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string formType, string? className, string? functionName, string? routeId, string? action, string? thisModel, string? type)
        {
            ViewBag.routeId = routeId;
            ViewBag.action = action;
            ViewBag.functionName = functionName;
            ViewBag.className = className;
            ViewBag.type = type;
            ViewBag.Model = thisModel;

            return View(formType);
        }
    }
}
