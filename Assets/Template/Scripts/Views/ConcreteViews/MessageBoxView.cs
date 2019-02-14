using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;

namespace Assets
{
	public sealed class MessageBoxView : View 
	{
        public class MessageBoxModel : InitPack
        {
            public string _title, _body, _buttonText;
            public Action _onResult;
        }

        public MessageBoxModel _model;

        public Text _titleText, _bodyText, _buttonText;

        public void ButtonClicked()
        {
            if (_model._onResult != null)
                _model._onResult();
        }

        public override void InitWithModel(InitPack model)
        {
            if (model == null)
                throw new ArgumentNullException("model cannot be null");
            var m = model as MessageBoxModel;
            if (m == null)
                throw new ArgumentException(GetType().ToString() + " cannot be initted with model of type " + model.GetType().ToString());
            _model = m;

            _buttonText.text = _model._buttonText;
            _bodyText.text = _model._body;
            _titleText.text = _model._title;
        }
    }
}