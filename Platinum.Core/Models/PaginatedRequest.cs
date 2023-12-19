using Anhny010920.Core.Attributes;
using Anhny010920.Core.ViewModels;
using System.ComponentModel;

namespace Platinum.Core.Models
{
    public class PaginatedRequest : IRequestCommand
    {
        public PaginatedRequest() { }
        private int page = 1;

        public PaginatedRequest(int page = 1, int take = 10)
        {
            Page = page == 0 ? 1 : page;
            Take = take;
        }

        [Minimum(1)]
        [DefaultValue(1)]
        public int Page
        {
            get
            {
                if (page == 0)
                    page = 1;
                return page;
            }
            set
            {
                page = value;
            }
        }

        [Minimum(1)]
        [Maximum(50)]
        [DefaultValue(10)]
        public int Take { get; set; }
    }
}
