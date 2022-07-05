using LY.Report.Core.Model.Auth.Enum;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.Permission.Input
{
    public class PermissionAddDotInput
    {
        /// <summary>
        /// Ȩ������
        /// </summary>
        public PermissionType Type { get; set; } = PermissionType.Dot;

        /// <summary>
        /// �����ڵ�
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// �����ӿ�
        /// </summary>
        public string[] ApiIds { get; set; }

        /// <summary>
        /// Ȩ������
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Ȩ�ޱ���
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// ˵��
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ͼ��
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// �ӿ������汾��
        /// </summary>
        [Display(Name = "Api�汾��")]
        [Required(ErrorMessage = "�汾�Ų���Ϊ��"), StringLength(20, ErrorMessage = "{0} ����Ϊ{2}-{1} ���ַ���", MinimumLength = 2)]
        public string ApiVersion { get; set; }
    }
}
