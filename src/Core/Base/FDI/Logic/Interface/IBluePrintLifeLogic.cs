namespace Framework.Core.FrameDI
{
    public interface IBluePrintLifeLogic
    {
        BluePrintRecord CreateBluePrint(string bluePrintName);
        void ReleaseBluePrint(string bluePrintName);
    }
}