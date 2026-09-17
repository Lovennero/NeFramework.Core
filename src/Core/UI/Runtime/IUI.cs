using System.Threading.Tasks;

namespace Framework.Core.UI
{
    public interface IUI
    {
        int SerialID { get; set; }
        
        void OnAcquire();
        ValueTask OnShow();
        
        void OnFocus();
        void OnUpdate(float logicTime,float realTime);
        void OnBlur();
        
        ValueTask OnHide();
        void OnRecycle();
    }
}