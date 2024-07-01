
using System.Collections.Generic;

namespace HappyCard
{
    /// <summary>
    /// �������
    /// </summary>
    public interface IVirtualPlayer
    {
        /// <summary>
        /// ��ǰ�Ƿ��Ѿ���������
        /// </summary>
        /// <remarks>���ֵ��GameLoop���п���</remarks>
        bool Finished { get; set; }

        /// <summary>
        /// ��ǰ��ҵĶ���
        /// </summary>
        /// <remarks>���ֵ��GameLoop���п���</remarks>
        IVirtualPlayer Teammate { get; set; }

        /// <summary>
        /// ����ʼ����ʱ����
        /// </summary>
        void OnDeal(List<PokerCard> cards);

        /// <summary>
        /// ����ǰ��ҵĻغ�
        /// </summary>
        /// <param name="last">�ϼһغϵ���Ϣ</param>
        void OnBout(Bout last);

        /// <summary>
        /// ��ǰ��ҵĻغϽ���ʱ����
        /// </summary>
        /// <param name="gameplay">��ǰ����Ϸ�淨</param>
        /// <param name="current">��ǰ����ҵĻغ�״̬</param>
        void OnEndBout(Gameplay gameplay, Bout current);

        /// <summary>
        /// ��ǰ��Ҵ������е���
        /// </summary>
        void OnPlayAllCards();

        /// <summary>
        /// ��ǰ��Ϸ����
        /// </summary>
        void OnGameOver(GameFile file);

        /// <summary>
        /// ��ӻغϼ�����
        /// </summary>
        IVirtualPlayer AddBoutListener(IBoutListener listener);
    }
}
