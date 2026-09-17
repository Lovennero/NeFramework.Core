namespace Framework.Core.Sound
{
    internal sealed class SoundStartLogic : ISoundStartLogic
    {
        private const string Tag = "Sound Star tLogic";

        private readonly ISoundStartHelper _helper;
        
        public SoundStartLogic(ISoundStartHelper helper)
        {
            _helper = helper;
        }
        
        public void Start()
        {
            // === 初始化根对象 ===
            _helper.InitSoundRoot();
            
            // === 初始化对象池 ===
            _helper.InitGroup();
        }
    }
}