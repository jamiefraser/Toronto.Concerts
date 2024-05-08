using Syncfusion.Maui.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toronto.Concerts.Data;

namespace Toronto.Concerts.MAUI.Behaviors
{
    public class ConcertGroupingBehavior : Behavior<Syncfusion.Maui.ListView.SfListView>
    {
        private Syncfusion.Maui.ListView.SfListView ListView;
        protected override void OnAttachedTo(Syncfusion.Maui.ListView.SfListView bindable)
        {
            ListView = bindable;
            ListView.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "GroupableDate",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Concert);
                    return item.GroupableDate.ToString();
                },
            });
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Syncfusion.Maui.ListView.SfListView bindable)
        {
            ListView = null;
            base.OnDetachingFrom(bindable);
        }
    }
}
