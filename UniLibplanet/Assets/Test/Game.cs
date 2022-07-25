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
             * ���� ���α׷��� ���� ���ü�� ���� ���� ��
             * �Ǵ� ������ �۾��� ����Ǵ� �Ͱ� ���� ���ü�� ���� �̺�Ʈ�� �����Ϸ���
             * ���� ���α׷��� Agentȣ���� �ݹ� �޼��� ������ �����ؾ� �մϴ�. 
             * �̸� ��������� �ϸ� IRenderer�������̽��� �����ؾ� �մϴ�.
             * ���� ������ ���ü���� ���� ����Ǹ� ���ü���� �޼ҵ带 ȣ���մϴ�.
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
