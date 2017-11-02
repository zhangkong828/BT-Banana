using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banana.Web.Models.ViewModels
{
    public class MagnetUrlSearchResultViewModel : BaseSearchResultViewModel<MagnetUrl>
    {
        public MagnetUrlSearchResultViewModel()
        {
            Result = new List<MagnetUrl>();
        }
    }
}
