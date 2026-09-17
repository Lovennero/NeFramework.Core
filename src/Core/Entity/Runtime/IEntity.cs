using System.Threading.Tasks;

namespace Framework.Core.Entity
{
    public interface IEntity
    {
        // === 基本参数 ===
        int SerialID { get; set; }

        // === 周期函数 ===
        ValueTask OnAcquire();
        ValueTask OnShow();
        void OnUpdate(float logicTime,float realTime);
        ValueTask OnHide();
        ValueTask OnRecycle();
    } 
}