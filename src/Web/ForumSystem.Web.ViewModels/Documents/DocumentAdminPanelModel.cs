using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Web.ViewModels.Documents
{
    public class DocumentAdminPanelModel
    {
        public IEnumerable<DocumentIndexModel> Documents { get; set; }
    }
}
