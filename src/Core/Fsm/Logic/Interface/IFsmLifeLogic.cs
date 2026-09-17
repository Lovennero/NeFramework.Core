
using System.Threading.Tasks;

namespace Framework.Core.Fsm
{
    public interface IFsmLifeLogic
    {
        FsmRecord CreateFsm(string fsmName, string scopeName);
        ValueTask RemoveFsm(string fsmName);
    }
}