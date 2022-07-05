using LY.Report.Core.Model.Auth.Enum;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.Permission.Input
{
    public class PermissionAddMenuInput
    {
        /// <summary>
        /// Ȩ������
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// �����ڵ�
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// ��ͼ
        /// </summary>
        public string ViewId { get; set; }

        /// <summary>
        /// ���ʵ�ַ
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Ȩ������
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// ˵��
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ����
        /// </summary>
		public bool Hidden { get; set; }

        ///// <summary>
        ///// ����
        ///// </summary>
        //public IsActive IsActive { get; set; }

        /// <summary>
        /// ͼ��
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// �ɹر�
        /// </summary>
        public bool? Closable { get; set; }

        /// <summary>
        /// ���´���
        /// </summary>
        public bool? NewWindow { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public bool? External { get; set; }

        /// <summary>
        /// �ӿ������汾��
        /// </summary>
        [Display(Name = "Api�汾��")]
        [Required(ErrorMessage = "�汾�Ų���Ϊ��"), StringLength(20, ErrorMessage = "{0} ����Ϊ{2}-{1} ���ַ���", MinimumLength = 2)]
        public string ApiVersion { get; set; }
    }
}
