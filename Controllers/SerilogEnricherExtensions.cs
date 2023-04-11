

using System.Security.Claims;
using Serilog.Core;
using Serilog.Events;


public class MemberIdEnricher : ILogEventEnricher
{
    private readonly HttpContext _context;

    public MemberIdEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _context = httpContextAccessor.HttpContext;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {

        //从上下文中获取 new Claim(ClaimTypes.Name, "user"),user就是memberId
       var memberId= _context?.User?.FindFirst(ClaimTypes.Name)?.Value;
      
            logEvent.AddPropertyIfAbsent(new LogEventProperty("MemberId", new ScalarValue(memberId)));
    }
}
