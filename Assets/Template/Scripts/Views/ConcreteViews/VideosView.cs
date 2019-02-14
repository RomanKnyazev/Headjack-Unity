using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Headjack;
using UnityEngine.UI;

namespace Assets
{
	public sealed class VideosView : ImageView
    {
        public Text CategoryName;

        public class VideosModel : InitPack
        {
            public App.CategoryMetadata _category;
        }

        public void Back()
        {
            HeadjackViewController.Instance.ShowCategories();
        }

        public override void InitWithModel(InitPack model)
        {
            if (model == null)
                throw new ArgumentNullException("model cannot be null");
            var m = model as VideosModel;
            if (m == null)
                throw new ArgumentException(GetType().ToString() + " cannot be initted with model of type " + model.GetType().ToString());
			if(CategoryName.text.Equals(null)  || CategoryName.text.Equals(""))
				CategoryName.text = "";
			else
				CategoryName.text = m._category.Name;

			var projects = (CategoryName.text.Equals("")) ? 
				  App.GetProjects ().Select(s => App.GetProjectMetadata(s)) 
				: App.GetProjects(m._category.Id).Select(s => App.GetProjectMetadata(s));

			foreach (var v in projects)
            {
                var id = v.Id;
                var img = App.GetImage(v.ThumbnailId);
                var item = AddItem(img, v.Title, () => HeadjackViewController.Instance.ShowVideoDetails(id));
                item.GetComponent<VideoOverview>().InitWithModel(new VideoOverview.VideoModel { _project = v });
            }
        }
    }
}