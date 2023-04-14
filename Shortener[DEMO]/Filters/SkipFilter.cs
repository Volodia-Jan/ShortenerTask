using Microsoft.AspNetCore.Mvc.Filters;

namespace Shortener_DEMO_.Filters;

//IFilterMetadata -> to have behavior as Filter
public class SkipFilter : Attribute, IFilterMetadata
{
}
