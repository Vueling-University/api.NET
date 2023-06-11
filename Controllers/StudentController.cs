using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        // GET: api/<StudentController>
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            using (SqlConnection cn = new SqlConnection("Server=.;Database=AngularApiDatabase;User Id=sa;Password=yourStrong(!)Password;"))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Student", cn))
                {
                    List<Student> students = new List<Student>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student student = new Student();
                            student.Id = reader.GetInt32(0);
                            student.Name = reader.GetString(1);
                            student.Surname = reader.GetString(2);
                            student.Age = reader.GetInt32(3);
                            students.Add(student);
                        }
                    }
                    return students;
                }
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Student> Get(int id)
        {
            using (SqlConnection cn = new SqlConnection("Server=.;Database=nombreBonito;User Id=sa;Password=yourStrong(!)Password;"))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE id = @id", cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Student student = new Student();
                            student.Id = reader.GetInt32(0);
                            student.Name = reader.GetString(1);
                            student.Surname = reader.GetString(2);
                            student.Age = reader.GetInt32(3);
                            return student;
                        }
                    }
                }
            }
            return NotFound();
        }

        // POST api/<StudentController>
        [HttpPost]
        public ActionResult<Student> Post([FromBody] Student newStudent)
        {
            using (SqlConnection cn = new SqlConnection("Server=.;Database=nombreBonito;User Id=sa;Password=yourStrong(!)Password;"))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Student (Name, Surname, Age) OUTPUT INSERTED.ID VALUES (@Name, @Surname, @Age)", cn))
                {
                    cmd.Parameters.AddWithValue("@Name", newStudent.Name);
                    cmd.Parameters.AddWithValue("@Surname", newStudent.Surname);
                    cmd.Parameters.AddWithValue("@Age", newStudent.Age);

                    int newId = (int)cmd.ExecuteScalar();
                    newStudent.Id = newId;
                }
            }
            return CreatedAtAction(nameof(Get), new { id = newStudent.Id }, newStudent);
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Student updatedStudent)
        {
            using (SqlConnection cn = new SqlConnection("Server=.;Database=nombreBonito;User Id=sa;Password=yourStrong(!)Password;"))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Student SET Name = @Name, Surname = @Surname, Age = @Age WHERE Id = @Id", cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", updatedStudent.Name);
                    cmd.Parameters.AddWithValue("@Surname", updatedStudent.Surname);
                    cmd.Parameters.AddWithValue("@Age", updatedStudent.Age);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }
            return NoContent();
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (SqlConnection cn = new SqlConnection("Server=.;Database=nombreBonito;User Id=sa;Password=yourStrong(!)Password;"))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Student WHERE Id = @Id", cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }
            return NoContent();
        }
    }
}
