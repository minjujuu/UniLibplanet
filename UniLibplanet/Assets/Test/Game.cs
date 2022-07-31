using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TMPro;

using Libplanet.Action;
using Libplanet.Blocks;
using Libplanet.Blockchain.Renderers;
using Libplanet.Unity;

namespace Scripts
{
    // Unity event handler.
    public class BlockUpdatedEvent : UnityEvent<Block<PolymorphicAction<ActionBase>>>
    {

    }

    public class Game : MonoBehaviour
    {
        
        public TextMeshProUGUI BlockHashText;
        public TextMeshProUGUI BlockIndexText;

        private BlockUpdatedEvent _blockUpdatedEvent;
        private IEnumerable<IRenderer<PolymorphicAction<ActionBase>>> _renderers;
        private Agent _agent;

        void Awake()
        {
            Screen.SetResolution(800, 600, FullScreenMode.Windowed);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);

            _blockUpdatedEvent = new BlockUpdatedEvent();
            _blockUpdatedEvent.AddListener(UpdateBlockTexts);

            /*
             * 응용 프로그램이 로컬 블록체인 팁의 변경 및
             * 또는 생성된 작업이 실행되는 것과 같은 블록체인 수준 이벤트에 반응하려면
             * 응용 프로그램이 Agent호출할 콜백 메서드 집합을 전달해야 합니다. 
             * 이를 렌더러라고 하며 IRenderer인터페이스를 구현해야 합니다.
             * 위의 예에서 블록체인의 끝이 변경되면 블록체인이 메소드를 호출합니다.
            */

            _renderers = new List<IRenderer<PolymorphicAction<ActionBase>>>()
            {
                new AnonymousRenderer<PolymorphicAction<ActionBase>>()
                {
                    BlockRenderer = (oldTip, newTip) =>
                    {
                        if(newTip.Index > 0)
                        {
                            _agent.RunOnMainThread(() => _blockUpdatedEvent.Invoke(newTip));
                        }
                    }
                }
            };

            _agent = Agent.AddComponentTo(gameObject, _renderers);

            
        }
    
        void Update()
        {
            BlockHashText.text = "Block Hash: 0000";
            BlockIndexText.text = "Block Index: 0";
        }

        private void UpdateBlockTexts(Block<PolymorphicAction<ActionBase>> tip)
        {
            BlockHashText.text = $"Block Hash: {tip.Hash.ToString().Substring(0, 4)}";
            BlockIndexText.text = $"Block Index: {tip.Index}";
        }
    }

}
