   M S B A M L   `   `   `  ����H  DEstudianteWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:EstudianteWPFEstudianteWPF  5          EstudianteWPF.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -localclr-namespace:EstudianteWPF     5        � Title$  Registro de Estudiantes��5      $	��700��6(   $
��1100��65   .��5	      � 
�������.+�5
   
   �� .@�5      �� $��Registro Estudiantes��6   .��5      � $��10q�6   
������.+�5      �� .+�5      �� -   5         ��Name$ ListaEstudiantes��5   %   
������.+�5      �� $#��Lista de Estudiantes en Linea��6$   $��Center=�6L   $��Center=�6i   $
��Bold"�6�   $��16(�6�   &   System.Windows.Controls.DataGrid  5      -   5          Name$ dgEstudiantes��5   '      AutoGenerateColumns$
 True��6>   $	��200��6Y   $��0,20,0,0q�6f   5   	   5      �� 5      -   5        $ Botones��5   %    �� Orientation$ 
Horizontal=�66   $��0,20,0,0q�6O   
������.+�5      �� -   5        $��btnNuevoRegistro��5   %   $��Nuevo Estudiante��6=   $��	10,10,0,0q�6t   
���/���6�   �� 5      -   5        $��btnAgregarMateria��5   %   $��Agregar Materia��6>   $��	10,10,0,0q�6u   
��� ���6�   �� 5      -   5        $��btnEditarMateria��5   %   $��Ver Materias Inscritas��6=   $��	10,10,0,0q�6z   
���2͚�6�   �� 5      -   5        $��btnEliminarEstudiante��5   %   $��Eliminar Estudiante��6B   $��	10,10,0,0q�6�   
���  ��6�   5      �� 5      $ 
Horizontal=�6%   $��0,20,0,0q�6?   
������.+�5      �� $��Nombre del Estudiante: ��6$   $��Center=�6F   $��Center=�6c   $
��Bold"�6~   �� 5      -   5         ��Name$ 	txtNombre��5   &   $	��200��69   $��10,0,0,0q�6E   �� 5      $��	Materia: ��6$   $
��Bold"�68   �� 5      -	   5         ��Name$ 
cbMaterias��5   '   $	��200��6;   $��10,0,0,0q�6G   6Y   �� 5       $��Profesor:  ��6$   $
��Bold"�6:   �� 5!      -
   5        $ cbProfesores��5!   '   $	��200��6=   $��10,0,0,0q�6I   5"      �� 5$      -   5        $ 
RegMateria��5$   %   $y�	Collapsed=�69   $��0,20,0,0q�6Q   
������.+�5%      �� $��Materias del Estudiante: ��6$   $
��Left=�6H   $��Center=�6c     5&      -   5        $ dgMateriasSeleccionadas��5&   '   $ False��6H      
IsReadOnly$
 True��6d   $	��100��6v   $��0,20,0,0q�6�      Columns 5'      0  *System.Windows.Controls.DataGridTextColumn  5(   "   ,  &System.Windows.Controls.DataGridColumn	   Header$	 Materia��65   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6F   �� *Nombre+  5)   "   $	 Profesor��65   
 6G   �� *ProfesorNombre+  5*   "   $	 Creditos��65   
 6G   �� *
Creditos+5,      �� 5-      -   5         ��Name$ btnGuardarMaterias��5-   %   $$��Guardar Materias de Estudiante��6A   $	��200��6j   $��0,10,0,0q�6v   
���/���6�   5.      �� 50      -   5        $
 Edit��50   %   $y�	Collapsed=�63   $��0,20,0,0q�6J   
����p��.+�51      �� $)��#Edita las materias del Estudiante: ��6$   $
��Left=�6R   $��Center=�6m     52      -   5        $ dgEstudianteMaterias��52   '   $
 True��6E   $	��100��6`   $��0,20,0,0q�6m   6   �� 53      -   5        $#��Editar Materia del Estudiante��53   %   $	��180��6v   $��0,10,0,0q�6�   
���/���6�   54      55      56      57      �� 5:      $��Clases Compartidas��6   .��5;      � $��10q�6   .+�5<        -   5        $ dgCompartidas��5<      $
 True��66   
����� �6R   5=      5>      5?      �� 5@   
   $/��)Desarrollado por: Jhon Jairo López Sáez��6   $��Right=�6B   $��Bottom=�6^   $��10q�6y   $��12(�6�   $��Italic$�6�   5A      5B      