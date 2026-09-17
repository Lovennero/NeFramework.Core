using System.Threading.Tasks;

namespace Framework.Core.Fsm
{
    public interface IState
    {
        void OnInit();
        ValueTask OnEnter();
        void OnUpdate(float logicTime,float realTime);
        ValueTask OnExit();
        void OnDestroy();
    }
}
