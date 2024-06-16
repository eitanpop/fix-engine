using QuickFix;

namespace Invinitive.FIX.Engine;

public interface IFixApplication : IApplication
{
    bool SendMessage(Message message);
}