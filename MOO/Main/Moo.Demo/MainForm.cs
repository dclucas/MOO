using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Moo.Mappers;

namespace Moo.Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            foreach (var t in GetTypeDescriptors())
            {
                cbxFrom.Items.Add(t);
                cbxTo.Items.Add(t);
            }

            cbxFrom.SelectedIndex = 0;
            cbxTo.SelectedIndex = 1;
            var from = (SampleClassA)pgrFrom.SelectedObject;
            from.Name = "Hello World!";
            from.ConfiguredPropertyA = 42;
            from.AttributedPropertyA = Math.PI;
            var mapper = MappingRepository.Default.ResolveMapper<SampleClassFullCustomer, SampleClassSimplifiedCustomer>();
            mapper.AddMappingAction(
                    "FirstName", 
                    "Name", 
                    (f, t) => t.Name = f.FirstName + " " + f.LastName
                    );

            // TODO: add a façade method to the repository class, so mapping rules can be added straight through it
            // the trouble of resolving the right mapper and keeping it should remain in the façade method.
        }

        private void cbxFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelection(cbxFrom, pgrFrom);
        }

        private void UpdateSelection(ComboBox cbx, PropertyGrid pgr)
        {
            var t = (TypeDescriptor)cbx.SelectedItem;
            pgr.SelectedObject = t.CreateInnerObject();
        }

        private IEnumerable<TypeDescriptor> GetTypeDescriptors()
        {
            return from t in Assembly.GetExecutingAssembly().GetTypes()
                   where t.Name.Contains("SampleClass")
                   select new TypeDescriptor() { InnerType = t };
        }

        public class TypeDescriptor
        {
            public Type InnerType { get; set; }

            public object CreateInnerObject()
            {
                return Activator.CreateInstance(InnerType);
            }

            public override string ToString()
            {
                return InnerType.Name;
            }
        }

        private void btnMap_Click(object sender, EventArgs e)
        {
            Type TSource = ((TypeDescriptor)cbxFrom.SelectedItem).InnerType;
            Type TTarget = ((TypeDescriptor)cbxTo.SelectedItem).InnerType;

            // HACK: this hack is necessary because there is no non-generic resolve method available.
            // if this ever becomes a requirement, add a non-generic resolve method targetMemberName this class.
            var resolveMethod = typeof(MappingRepository).GetMethod("ResolveMapper", BindingFlags.Public | BindingFlags.Instance);
            resolveMethod = resolveMethod.MakeGenericMethod(new Type[] { TSource, TTarget });
            var mapper = resolveMethod.Invoke(MappingRepository.Default, null);
            var mapMethod = mapper.GetType().GetMethod("Map", new Type[] { TSource, TTarget });
            mapMethod.Invoke(mapper, new Object[] { pgrFrom.SelectedObject, pgrTo.SelectedObject });

            pgrTo.SelectedObject = pgrTo.SelectedObject;
        }

        private void cbxTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelection(cbxTo, pgrTo);
        }

        private void WikiSample1()
        {
            SourceClass source = new SourceClass();
            source.Title = "Hello World!";
            TargetClass target = new TargetClass();
            var mapper = MappingRepository.Default.ResolveMapper<SourceClass, TargetClass>();
            mapper.Map(source, target);
        }

        private void WikiSample2()
        {
            SourceClass source = new SourceClass();
            source.Title = "Hello World!";
            TargetClass target = new TargetClass();
            ConventionMapper<SourceClass, TargetClass> mapper = new ConventionMapper<SourceClass, TargetClass>();
            mapper.Map(source, target);

        }

        private void WikiSample3()
        {
            SourceClass source = new SourceClass();
            source.Title = "Hello World!";
            TargetClass target = new TargetClass();
            CompositeMapper<SourceClass, TargetClass> mapper =
                new CompositeMapper<SourceClass, TargetClass>(
                    new ConfigurationMapper<SourceClass, TargetClass>(),
                    new ConventionMapper<SourceClass, TargetClass>()
                    );
            mapper.Map(source, target);
        }

        private void WikiSample4()
        {
            SourceClass source = new SourceClass();
            source.Title = "Hello World!";
            TargetClass target = new TargetClass();
            ManualMapper<SourceClass, TargetClass> mapper = new ManualMapper<SourceClass, TargetClass>();
            mapper.AddMappingAction("Title", "Title", (s, t) => t.Title = "SourceMemberName: " + s.Title);
            mapper.Map(source, target);
        }

        private class SourceClass
        {

            public string Title { get; set; }
        }

        private class TargetClass
        {
            public string Title { get; set; }
        }
    }
}
