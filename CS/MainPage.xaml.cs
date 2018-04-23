using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Grid;
using DevExpress.Data.Browsing;
using System.Diagnostics;
using GridLayoutHelper;

namespace DXGridSample.SL {
	public partial class MainPage: UserControl {
		ObservableCollection<Person> Persons;
		public MainPage() {
			InitializeComponent();
			Persons = new ObservableCollection<Person>();
			for (int i = 0; i < 100; i++)
				Persons.Add(new Person { Id = i, Name = "Name" + i, Bool = i % 2 == 0 });
			grid.ItemsSource = Persons;
		}
		private void AddColumn_Click(object sender, System.Windows.RoutedEventArgs e) {
			grid.Columns.Add(new GridColumn() { FieldName = "Name" });
		}
		private void RemoveColumn_Click(object sender, System.Windows.RoutedEventArgs e) {
			grid.Columns.Remove(grid.Columns.Last());
		}
		private void GridLayoutHelper_Trigger(object sender, MyEventArgs e) {
			string str = string.Empty;
			foreach (var type in e.LayoutChangedTypes)
				str = string.IsNullOrEmpty(str) ? type.ToString() : str + " | " + type;
			Debug.WriteLine(str);
		}
	}
	public class Person {
		public int Id { get; set; }
		public string Name { get; set; }
		public bool Bool { get; set; }
	}
}