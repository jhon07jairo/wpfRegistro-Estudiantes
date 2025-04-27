using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EstudianteWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		CargarMaterias();
		CargarEstudiantes();
		CargarProfesores();
		CargarClasesCompartidas();
		//CargarEstudianteMaterias();
	}
	//string conexion = ConfigurationManager.ConnectionStrings["ConexionSql"].ConnectionString;
	SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-K5NSNM7\SQLEXPRESS;Initial Catalog=RegistroEstudiantesDB;Integrated Security=True;TrustServerCertificate=True");
	List<dynamic> materiasSeleccionadas = new();
	int? estudianteEditandoId = null;
	int? materiaSeleccionadaId = null; // Para actualizar una sola materia	

	// Cargar Materias
	private void CargarMaterias()
	{
		//using SqlConnection con = new(conexion);
		con.Open();

		SqlCommand cmd = new("SELECT * FROM Materia", con);
		SqlDataReader reader = cmd.ExecuteReader();
		var lista = new List<dynamic>();

		while (reader.Read())
		{
			lista.Add(new
			{
				Id = reader["Id"],
				Nombre = reader["Nombre"]
			});
		}

		cbMaterias.ItemsSource = lista;
		cbMaterias.DisplayMemberPath = "Nombre";
		cbMaterias.SelectedValuePath = "Id";

		con.Close();
	}

	// Update the AgregarMateria_Click method to cast the selected items to dynamic
	private void AgregarMateria_Click(object sender, RoutedEventArgs e)
	{
		if (materiasSeleccionadas.Count >= 3)
		{
			MessageBox.Show("El estudiante no puede seleccionar más de 3 materias.");
			return;
		}

		if (cbMaterias.SelectedItem == null || cbProfesores.SelectedItem == null)
		{
			MessageBox.Show("Debe seleccionar una materia y un profesor.");
			return;
		}

		// Cast the selected items to dynamic to access their properties
		dynamic materia = cbMaterias.SelectedItem;
		dynamic profesor = cbProfesores.SelectedItem;
		int creditos = 3;

		// Validar que no se repita el profesor  
		foreach (var seleccion in materiasSeleccionadas)
		{
			if (seleccion.ProfesorId == profesor.Id)
			{
				MessageBox.Show("No puede seleccionar el mismo profesor para diferentes materias.");
				return;
			}
		}

		materiasSeleccionadas.Add(new
		{
			Id = materia.Id,
			Nombre = materia.Nombre,
			ProfesorId = profesor.Id,
			ProfesorNombre = profesor.Nombre,
			Creditos = creditos
		});

		dgMateriasSeleccionadas.ItemsSource = null; // Resetear DataGrid  
		dgMateriasSeleccionadas.ItemsSource = materiasSeleccionadas;

		if (dgMateriasSeleccionadas.Items.Count >= 3)
		{
			btnAgregarMateria.IsEnabled = false;
			btnGuardarMaterias.IsEnabled = true;
		}
	}

	// Cargar Profesores en el ComboBox  
	private void CargarProfesores()
	{
		con.Open();

		SqlCommand cmd = new("SELECT * FROM Profesor", con);
		SqlDataReader reader = cmd.ExecuteReader();
		var lista = new List<dynamic>();

		while (reader.Read())
		{
			lista.Add(new
			{
				Id = reader["Id"],
				Nombre = reader["Nombre"]
			});
		}

		cbProfesores.ItemsSource = lista;
		cbProfesores.DisplayMemberPath = "Nombre";
		cbProfesores.SelectedValuePath = "Id";

		con.Close();
	}

	// Guardar Estudiante
	private void GuardarEstudiante_Click(object sender, RoutedEventArgs e)
	{
		if (string.IsNullOrEmpty(txtNombre.Text))
		{
			MessageBox.Show("El nombre del estudiante es obligatorio.");
			return;
		}

		if (materiasSeleccionadas.Count != 3)
		{
			MessageBox.Show("El estudiante debe seleccionar exactamente 3 materias.");
			return;
		}

		//using SqlConnection con = new(conexion);
		con.Open();

		//SqlTransaction transaction = con.BeginTransaction();

		try
		{
			// Validar si el estudiante ya existe
			SqlCommand cmdEstudianteExistente = new SqlCommand("SELECT COUNT(*) FROM Estudiante WHERE Nombre = @Nombre", con);
			cmdEstudianteExistente.Parameters.AddWithValue("@Nombre", txtNombre.Text);
			int estudianteExistente = Convert.ToInt32(cmdEstudianteExistente.ExecuteScalar());

			if (estudianteExistente > 0)
			{
				MessageBox.Show("El estudiante ya existe en la base de datos.");
				return;
			}
			// Insertar Estudiante
			SqlCommand cmdEstudiante = new("INSERT INTO Estudiante (Nombre) VALUES (@Nombre); SELECT SCOPE_IDENTITY()", con);
			cmdEstudiante.Parameters.AddWithValue("@Nombre", txtNombre.Text);
			int estudianteId = Convert.ToInt32(cmdEstudiante.ExecuteScalar());

			// Insertar Materias
			foreach (var materia in materiasSeleccionadas)
			{
				SqlCommand cmdMateria = new("INSERT INTO EstudianteMateria (EstudianteId, MateriaId, ProfesorId) VALUES (@EstudianteId, @MateriaId, @ProfesorId)", con);
				cmdMateria.Parameters.AddWithValue("@EstudianteId", estudianteId);
				cmdMateria.Parameters.AddWithValue("@MateriaId", materia.Id);
				cmdMateria.Parameters.AddWithValue("@ProfesorId", materia.ProfesorId);
				cmdMateria.ExecuteNonQuery();
			}

			//transaction.Commit();
			MessageBox.Show("Estudiante guardado correctamente.");
			LimpiarFormulario();
			con.Close();
			CargarEstudiantes();
			con.Close();
		}
		catch (Exception ex)
		{
			//transaction.Rollback();
			MessageBox.Show("Error al guardar el estudiante: " + ex.Message);
		}
		finally
		{
			con.Close(); // Asegurarse de cerrar la conexión
		}
	}


	// Limpiar formulario después de guardar, actualizar o eliminar
	private void LimpiarFormulario()
	{
		txtNombre.Clear();
		materiasSeleccionadas.Clear();
		dgMateriasSeleccionadas.ItemsSource = null;
		estudianteEditandoId = null;
		cbMaterias.SelectedItem = null;
		cbProfesores.SelectedItem = null;
	}

	// Cargar Estudiantes en el DataGrid
	private void CargarEstudiantes()
	{
		//using SqlConnection con = new(conexion);
		con.Open();

		SqlCommand cmd = new("SELECT * FROM Estudiante", con);
		SqlDataReader reader = cmd.ExecuteReader();
		var lista = new List<dynamic>();

		while (reader.Read())
		{
			lista.Add(new
			{
				Id = reader["Id"],
				Nombre = reader["Nombre"]
			});
		}

		dgEstudiantes.ItemsSource = lista;

		con.Close();
	}

	// Evento para seleccionar un estudiante en el DataGrid para editar
	private void dgEstudiantes_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		dynamic estudiante = dgEstudiantes.SelectedItem;
		if (estudiante != null)
		{
			estudianteEditandoId = estudiante.Id;
			txtNombre.Text = estudiante.Nombre;
			CargarMateriasSeleccionadas(estudiante.Id);
			CargarEstudianteMaterias(estudiante.Id);
			btnEditarMateria.IsEnabled = true;
		}
	}

	// Cargar Materias seleccionadas para un estudiante
	private void CargarMateriasSeleccionadas(int estudianteId)
	{
		//using SqlConnection con = new(conexion);
		con.Open();

		SqlCommand cmd = new(@"
			SELECT m.Id, m.Nombre, p.Id AS ProfesorId, p.Nombre AS ProfesorNombre, m.Creditos
			FROM EstudianteMateria em
			JOIN Materia m ON em.MateriaId = m.Id
			JOIN Profesor p ON em.ProfesorId = p.Id	
			WHERE em.EstudianteId = @EstudianteId", con);
		cmd.Parameters.AddWithValue("@EstudianteId", estudianteId);

		SqlDataReader reader = cmd.ExecuteReader();
		materiasSeleccionadas.Clear();

		while (reader.Read())
		{
			materiasSeleccionadas.Add(new
			{
				Id = reader["Id"],
				Nombre = reader["Nombre"],
				ProfesorId = reader["ProfesorId"],
				ProfesorNombre = reader["ProfesorNombre"],
				Creditos = reader["Creditos"]
			});
		}

		dgMateriasSeleccionadas.ItemsSource = null;
		dgMateriasSeleccionadas.ItemsSource = materiasSeleccionadas;
		con.Close();

	}


	private void CargarClasesCompartidas()
	{
		//using SqlConnection con = new(conexion);
		con.Open();

		SqlCommand cmd = new(@"SELECT e1.Nombre AS Estudiante1, e2.Nombre AS Estudiante2, m.Nombre AS Materia
										FROM EstudianteMateria em1
										INNER JOIN EstudianteMateria em2 ON em1.MateriaId = em2.MateriaId AND em1.EstudianteId <> em2.EstudianteId
										INNER JOIN Estudiante e1 ON em1.EstudianteId = e1.Id
										INNER JOIN Estudiante e2 ON em2.EstudianteId = e2.Id
										INNER JOIN Materia m ON m.Id = em1.MateriaId", con);

		SqlDataReader reader = cmd.ExecuteReader();
		var lista = new List<dynamic>();

		while (reader.Read())
		{
			lista.Add(new
			{
				Estudiante = reader["Estudiante1"].ToString(),
				CompartenCon = reader["Estudiante2"].ToString(),
				Materia = reader["Materia"].ToString()
			});
		}

		dgCompartidas.ItemsSource = lista;

		con.Close();
	}

	private void cbMaterias_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (cbMaterias.SelectedItem == null)
			return;

		dynamic materiaSeleccionada = cbMaterias.SelectedItem;
		int materiaId = materiaSeleccionada.Id;

		con.Open();
		SqlCommand cmd = new("SELECT p.Id, p.Nombre FROM Profesor p INNER JOIN Materia m ON p.Id = m.ProfesorId WHERE m.Id = @MateriaId", con);
		cmd.Parameters.AddWithValue("@MateriaId", materiaId);

		var lista = new List<dynamic>();
		SqlDataReader reader = cmd.ExecuteReader();

		while (reader.Read())
		{
			lista.Add(new
			{
				Id = reader["Id"],
				Nombre = reader["Nombre"]
			});
		}

		cbProfesores.ItemsSource = lista;
		cbProfesores.DisplayMemberPath = "Nombre";
		cbProfesores.SelectedValuePath = "Id";

		if (lista.Count == 1)
		{
			cbProfesores.SelectedIndex = 0; // Selecciona automáticamente si solo hay un profesor
		}

		con.Close();
	}


	private void btnEditarEstudianteMateria_Click(object sender, RoutedEventArgs e)
	{
		if (dgEstudianteMaterias.SelectedItem == null)
		{
			MessageBox.Show("Selecciona un registro de EstudianteMateria para editar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			return;
		}

		// Abrimos conexión
		con.Open();

		// Preparamos el comando SQL para comprobar si el profesor ya está asignado a la materia
		var filaSeleccionada = dgEstudianteMaterias.SelectedItem as DataRowView;
		if (filaSeleccionada == null)
		{
			MessageBox.Show("Error al obtener el registro seleccionado.");
			con.Close();
			return;
		}

		int idEstudianteMateria = Convert.ToInt32(filaSeleccionada["Id"]);
		int idMateria = Convert.ToInt32(cbMaterias.SelectedValue);
		int idProfesor = Convert.ToInt32(cbProfesores.SelectedValue);

		// Verificar si el mismo profesor ya está asignado a una diferente materia ahora
		SqlCommand cmdVerificarProfesor = new SqlCommand(
		   @"SELECT COUNT(*) 
     FROM EstudianteMateria 
     WHERE ProfesorId = @IdProfesor AND MateriaId <> @IdMateria AND EstudianteId = @EstudianteId", con);
		cmdVerificarProfesor.Parameters.AddWithValue("@IdProfesor", idProfesor);
		cmdVerificarProfesor.Parameters.AddWithValue("@IdMateria", idMateria);
		cmdVerificarProfesor.Parameters.AddWithValue("@EstudianteId", estudianteEditandoId ?? (object)DBNull.Value);

		int count = Convert.ToInt32(cmdVerificarProfesor.ExecuteScalar());

		if (count > 0)
		{
			MessageBox.Show("El profesor ya está asignado a otra materia para este estudiante.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			con.Close();
			return;
		}

		

		// Preparamos el comando SQL para actualizar
		SqlCommand cmdActualizar = new SqlCommand(
			"UPDATE EstudianteMateria SET MateriaId = @IdMateria, ProfesorId = @IdProfesor WHERE Id = @Id", con);

		// Asignamos parámetros para la actualización
		cmdActualizar.Parameters.AddWithValue("@IdMateria", cbMaterias.SelectedValue ?? (object)DBNull.Value);
		cmdActualizar.Parameters.AddWithValue("@IdProfesor", cbProfesores.SelectedValue ?? (object)DBNull.Value);
		cmdActualizar.Parameters.AddWithValue("@Id", idEstudianteMateria);

		try
		{
			cmdActualizar.ExecuteNonQuery();
			MessageBox.Show("EstudianteMateria actualizado correctamente.", "Actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
		}
		catch (SqlException ex)
		{
			MessageBox.Show("Error al actualizar: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		finally
		{
			con.Close();
			// Aquí refrescas el grid
			CargarEstudianteMaterias(idEstudianteMateria);
			LimpiarCamposEstudianteMateria();
		}
		
	}


	private void dgEstudianteMaterias_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (dgEstudianteMaterias.SelectedItem is DataRowView row)
		{
			//estudianteMateriaEditandoId = Convert.ToInt32(row["Id"]);

			cbMaterias.SelectedValue = Convert.ToInt32(row["MateriaId"]);
			cbProfesores.SelectedValue = Convert.ToInt32(row["ProfesorId"]);
		}
	}
	private void CargarEstudianteMaterias(int estudianteId)
	{
		con.Open();
		SqlCommand cmd = new SqlCommand(
			@"SELECT 
            em.Id, 
            em.MateriaId, 
            m.Nombre AS MateriaNombre, 
            em.ProfesorId, 
            p.Nombre AS ProfesorNombre
          FROM EstudianteMateria em
          INNER JOIN Materia m ON em.MateriaId = m.Id
          INNER JOIN Profesor p ON em.ProfesorId = p.Id
          WHERE em.EstudianteId = @EstudianteId", con);

		cmd.Parameters.AddWithValue("@EstudianteId", estudianteId);

		SqlDataAdapter da = new SqlDataAdapter(cmd);
		DataTable dt = new DataTable();
		da.Fill(dt);
		dgEstudianteMaterias.ItemsSource = dt.DefaultView;
		con.Close();
	}

	private void LimpiarCamposEstudianteMateria()
	{
		cbMaterias.SelectedIndex = -1;
		cbProfesores.SelectedIndex = -1;
		dgEstudianteMaterias.SelectedItem = null;
	}

	private void EditarMateria_Click(object sender, RoutedEventArgs e)
	{
		RegMateria.Visibility = Visibility.Collapsed;
		Edit.Visibility = Visibility.Visible;
		btnAgregarMateria.IsEnabled = false;
		txtNombre.IsEnabled = false;
	}

	private void NuevoRegistro_Click(object sender, RoutedEventArgs e)
	{
		RegMateria.Visibility = Visibility.Visible;
		Edit.Visibility = Visibility.Collapsed;
		txtNombre.IsEnabled = true;
		txtNombre.Text = "";
		materiasSeleccionadas.Clear();
		dgMateriasSeleccionadas.ItemsSource = null;
		btnAgregarMateria.IsEnabled = true;
		btnEditarMateria.IsEnabled = false;
		btnGuardarMaterias.IsEnabled = false;

	}

	private void EliminarEstudiante_Click(object sender, RoutedEventArgs e)
	{
		if (dgEstudiantes.SelectedItem == null)
		{
			MessageBox.Show("Debe seleccionar un estudiante para eliminar.");
			return;
		}

		dynamic estudianteSeleccionado = dgEstudiantes.SelectedItem;
		int estudianteId = estudianteSeleccionado.Id;

		con.Open();
		try
		{
			// Eliminar materias asociadas al estudiante
			SqlCommand cmdEliminarMaterias = new("DELETE FROM EstudianteMateria WHERE EstudianteId = @EstudianteId", con);
			cmdEliminarMaterias.Parameters.AddWithValue("@EstudianteId", estudianteId);
			cmdEliminarMaterias.ExecuteNonQuery();

			// Eliminar estudiante
			SqlCommand cmdEliminarEstudiante = new("DELETE FROM Estudiante WHERE Id = @Id", con);
			cmdEliminarEstudiante.Parameters.AddWithValue("@Id", estudianteId);
			cmdEliminarEstudiante.ExecuteNonQuery();

			MessageBox.Show("Estudiante eliminado correctamente.");
			con.Close();
			CargarEstudiantes(); // Actualizar la lista de estudiantes
			con.Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error al eliminar el estudiante: " + ex.Message);
		}
		finally
		{
			con.Close();
		}
	}

}