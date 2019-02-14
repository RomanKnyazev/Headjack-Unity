using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Headjack;

namespace Assets
{
	public sealed class CategoriesView : ImageView
    {
        public class CategoriesModel : InitPack
        {
            public IEnumerable<App.CategoryMetadata> _categories;
        }

        public override void InitWithModel(InitPack model)
        {
            if (model == null)
                throw new ArgumentNullException("model cannot be null");
            var m = model as CategoriesModel;
            if (m == null)
                throw new ArgumentException(GetType().ToString() + " cannot be initted with model of type " + model.GetType().ToString());

            foreach (var i in m._categories)
            {
                var cat = i.Id;
                var img = App.GetImage(i.ThumbnailId);
                AddItem(img, i.Name, () => HeadjackViewController.Instance.ShowVideos(cat));
            }
        }

    }
}