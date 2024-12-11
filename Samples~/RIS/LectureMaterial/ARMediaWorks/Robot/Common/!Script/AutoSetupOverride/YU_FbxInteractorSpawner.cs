using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Serialization;

namespace CWJ.YU.Mobility
{
    public class YU_FbxInteractorSpawner : _FbxInteractorSpawner
    {
        [HelpBox(nameof(targetTopicNumber) + "는 숫자1부터 시작함")]
        [FormerlySerializedAs("targetTopicIndex")]
        public int targetTopicNumber;

        protected override Transform GetInteractorParent()
        {
            int topicIndex = targetTopicNumber - 1;
            var topic = FindObjectsOfType<Topic>().FirstOrDefault(t => t.topicIndex == topicIndex);
            if (!topic)
            {
                return null;
            }
            return topic.fbxRootLocateTrf;
        }

    }
}
