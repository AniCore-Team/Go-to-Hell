using System;
using System.Collections.Generic;

namespace Config
{
    [Serializable]
    public struct TextDialogueStep
    {
        public string nameStep;
        public List<TextPair> texts;
    }

    [Serializable]
    public struct AudioDialogueStep
    {
        public string nameStep;
        public List<AudioPair> audios;
    }
}