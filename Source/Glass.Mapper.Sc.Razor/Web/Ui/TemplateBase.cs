﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Web.Mvc;
using Glass.Mapper.Sc.Razor.Web.Mvc;
using RazorEngine.Text;
using Sitecore.Web.UI.WebControls;
using Image = Glass.Mapper.Sc.Fields.Image;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    /// <summary>
    /// Class TemplateBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TemplateBase<T> : RazorEngine.Templating.TemplateBase<T>
    {
        private HtmlHelper _helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateBase{T}"/> class.
        /// </summary>
        public TemplateBase()
        {


        }

        public ISitecoreContext SitecoreContext { get; set; }

        /// <summary>
        /// Gets the view data.
        /// </summary>
        /// <value>The view data.</value>
        public ViewDataDictionary ViewData { get; private set; }


        private GlassHtmlFacade _glassHtml;

        /// <summary>
        /// Gets the glass HTML.
        /// </summary>
        /// <value>The glass HTML.</value>
        public GlassHtmlFacade GlassHtml
        {
            get
            {
                if (_glassHtml == null)
                    _glassHtml = new GlassHtmlFacade(SitecoreContext, this.CurrentWriter);

                return _glassHtml;
            }
        }

        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <value>The HTML.</value>
        public HtmlHelper Html { get; private set; }

        /// <summary>
        /// Gets the placeholders.
        /// </summary>
        /// <value>The placeholders.</value>
        public IEnumerable<Placeholder> Placeholders
        {
            get
            {
                return ParentControl.Controls.Cast<Control>()
                                    .Where(x => x is global::Sitecore.Web.UI.WebControls.Placeholder)
                                    .Cast<global::Sitecore.Web.UI.WebControls.Placeholder>();
            }
        }

        /// <summary>
        /// Gets the parent control.
        /// </summary>
        /// <value>The parent control.</value>
        public Control ParentControl { get; private set; }

        /// <summary>
        /// Configures the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="viewData">The view data.</param>
        /// <param name="parentControl">The parent control.</param>
        public void Configure(ISitecoreContext sitecoreContext, ViewDataDictionary viewData, Control parentControl)
        {
            SitecoreContext = sitecoreContext;

            Html = new HtmlHelper(new ViewContext(), new ViewDataContainer() {ViewData = ViewData});
            ViewData = viewData;
            ParentControl = parentControl;
        }

        /// <summary>
        /// Renders the holder.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public string RenderHolder(string key)
        {
            key = key.ToLower();
            var placeHolder = Placeholders.FirstOrDefault(x => x.Key == key);

            if (placeHolder == null)
                return "No placeholder with key: {0}".Formatted(key);
            else
            {
                var sb = new StringBuilder();
                placeHolder.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Placeholders the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>IEncodedString.</returns>
        public IEncodedString Placeholder(string key)
        {
            key = key.ToLower();
            var placeHolder = Placeholders.FirstOrDefault(x => x.Key == key);

            if (placeHolder == null)
                placeHolder = new global::Sitecore.Web.UI.WebControls.Placeholder {Key = key};
            ParentControl.Controls.Add(placeHolder);

            var sb = new StringBuilder();
            placeHolder.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
            return Raw(sb.ToString());
        }

        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <returns>IEncodedString.</returns>
        public IEncodedString Editable<T1>(T1 target, Expression<Func<T1, object>> field)
        {
            return GlassHtml.Editable(target, field);
        }

        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <returns>IEncodedString.</returns>
        public IEncodedString Editable<T1>(T1 target, Expression<Func<T1, object>> field,
                                           Expression<Func<T1, string>> standardOutput)
        {
            return GlassHtml.Editable(target, field);
        }

        /// <summary>
        /// Editables the specified target.
        /// </summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        /// <param name="target">The target.</param>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IEncodedString.</returns>
        public IEncodedString Editable<T1>(T1 target, Expression<Func<T1, object>> field, string parameters)
        {
            return GlassHtml.Editable(target, field, parameters);
        }


        /// <summary>
        /// Renders the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>IEncodedString.</returns>
        public IEncodedString RenderImage(Image image)
        {
            return GlassHtml.RenderImage(image);
        }

        /// <summary>
        /// Renders the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>IEncodedString.</returns>
        public IEncodedString RenderImage(Image image, NameValueCollection attributes)
        {
            return GlassHtml.RenderImage(image, attributes);
        }



        /// <summary>
        /// Editables the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>RawString.</returns>
        /// <exception cref="System.NullReferenceException">
        /// No field set
        /// or
        /// No model set
        /// </exception>
        public RawString Editable(Expression<Func<T, object>> field)
        {
            if (field == null) throw new NullReferenceException("No field set");

            if (Model == null) throw new NullReferenceException("No model set");

            try
            {
                return GlassHtml.Editable(this.Model, field);
            }
            catch (Exception ex)
            {
                return new RawString(ex.Message);
            }

        }

        /// <summary>
        /// Editables the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>RawString.</returns>
        /// <exception cref="System.NullReferenceException">
        /// No field set
        /// or
        /// No model set
        /// </exception>
        public RawString Editable(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)
        {
            if (field == null) throw new NullReferenceException("No field set");

            if (Model == null) throw new NullReferenceException("No model set");

            if (standardOutput == null) throw new NullReferenceException("No standard output set");

            try
            {
                return GlassHtml.Editable(this.Model, field, standardOutput);
            }
            catch (Exception ex)
            {
                return new RawString(ex.Message);
            }

        }

        public bool IsInEditingMode
        {
            get { return Sc.GlassHtml.IsInEditingMode; }

        }
    }
}