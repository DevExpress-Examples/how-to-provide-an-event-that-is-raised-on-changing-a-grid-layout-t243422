<!-- default file list -->
*Files to look at*:

* [GridLayoutHelper.cs](./CS/GridLayoutHelper.cs) (VB: [GridLayoutHelper.vb](./VB/GridLayoutHelper.vb))
* [MainPage.xaml](./CS/MainPage.xaml) (VB: [MainPage.xaml](./VB/MainPage.xaml))
* [MainPage.xaml.cs](./CS/MainPage.xaml.cs) (VB: [MainPage.xaml.vb](./VB/MainPage.xaml.vb))
<!-- default file list end -->
# How to provide an event that is raised on changing a grid layout


<p>This example demonstrates how to provide an event that is raised as soon as a grid layout is changed.</p>
<br />
<p>In this example, we have created a GridLayoutHelper class that handles changes of the GridControl object's necessary properties and columns, and raises a custom event in its handlers.</p>
<br />
<p>This class can be easily attached to the GridControl object in the following manner:</p>


```xaml
<dxg:GridControl x:Name="grid" AutoPopulateColumns="True">
	<dxmvvm:Interaction.Behaviors>
		<local:GridLayoutHelper LayoutChanged="OnGridLayoutChanged"/>
	</dxmvvm:Interaction.Behaviors>
</dxg:GridControl>
```


<p> </p>
<p>To learn more on how to implement similar functionality in <strong>WPF</strong>, refer to the <a href="https://www.devexpress.com/Support/Center/p/E4609">E4609</a> example.</p>

<br/>


