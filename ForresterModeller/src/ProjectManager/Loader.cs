using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;

namespace ForresterModeller.src.ProjectManager
{
    public static class Loader
    {


        /// <summary>
        /// Получает на вход путь к файлу проекта и по нему инициализирует экземпляр проекта
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Project InitProjectByPath(string path)
        {
   
            Project project = new Project(Path.GetFileNameWithoutExtension(path), Path.GetDirectoryName(path));
            try
            {
                StreamReader r = new StreamReader(path);
                string json = r.ReadToEnd();

                var jobj = JsonObject.Parse(json);
                r.Close();
                project.FromJson(jobj.AsObject());

                var options = new JsonSerializerOptions { WriteIndented = true };////////////////////
                MessageBox.Show(jobj.ToJsonString(options));//////////////////////////////
            }
            catch { MessageBox.Show("Ошибка файла проекта"); }

            return project;

        }
        /// <summary>
        /// Создать новую директорию (не важно, для целого проекта или в самом проекте)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //пример вызова:   CreateDirectory(path + "\\" + "имя новой папки");
        public static string CreateDirectory(string path)
        {
            int index = 0;
            if (Directory.Exists(path))
            {
                index = 1;

                while (Directory.Exists(path + index))
                {
                    index++;
                }
                path = path + index;

            }
            Directory.CreateDirectory(path);
            return (index == 0) ? "" : index.ToString();
        }


        /// <summary>
        /// Создать файл json для записи
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CreateFile(string filename, string path)
        {
            if (File.Exists(path + "\\" + filename + ".json"))
            {
                int index = 1;
                while (File.Exists(path + "\\" + filename + index + ".json"))
                {
                    index++;
                }
                filename = filename + index;
            }
            StreamWriter file = File.CreateText(path + "\\" + filename + ".json");
            file.Close();
            return filename;
        }


        /// <summary>
        /// запись в файл json, причем он перезаписывает файл
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <param name="inf"></param>
        public static void WriteFileJson(string filename, string path, JsonObject inf)
        {
            using (StreamWriter file = new StreamWriter(path + "\\" + filename + ".json", false))
            {

                file.Write(inf);
                file.Close();

            }
        }

    }
}
