using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MoYobuDb.Models.Database;
using MoYobuDb.Models.Database.Dao;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;


namespace MoYobuDb.Models
{
    public class Helper
    {
        public static int mediaPerPage = 21;

        public static List<SelectListItem> GetAnimeFormats()
        {
            AnimeDao animeDao = new AnimeDao();
            List<SelectListItem> l = new List<SelectListItem>();

            foreach (var item in animeDao.FindFormats())
            {
                l.Add(new SelectListItem {Value = item, Text = item});
            }
            
            return l;
        }
        
        public static List<SelectListItem> GetMangaFormats()
        {
            MangaDao mangaDao = new MangaDao();
            List<SelectListItem> l = new List<SelectListItem>();

            foreach (var item in mangaDao.FindFormats())
            {
                l.Add(new SelectListItem {Value = item, Text = item});
            }
            
            return l;
        }
        
        public static List<SelectListItem> GetMangaStatus()
        {
            MangaDao mangaDao = new MangaDao();
            List<SelectListItem> l = new List<SelectListItem>();

            foreach (var item in mangaDao.FindStatus())
            {
                l.Add(new SelectListItem {Value = item, Text = item});
            }
            
            return l;
        }
        
        public static List<SelectListItem> GetMangaScore()
        {
            MangaDao mangaDao = new MangaDao();
            List<SelectListItem> l = new List<SelectListItem>();

            foreach (var item in mangaDao.FindScore())
            {
                l.Add(new SelectListItem {Value = item, Text = item});
            }
            
            return l;
        }
        
        public static List<SelectListItem> GetAnimeYears()
        {
            AnimeDao animeDao = new AnimeDao();
            List<SelectListItem> l = new List<SelectListItem>();

            foreach (var item in animeDao.FindYears())
            {
                if (Convert.ToInt32(item) > 1979 && Convert.ToInt32(item) <= DateTime.Now.Year)
                    l.Add(new SelectListItem {Value = item, Text = item});
            }
            
            return l;
        }
        
        public static List<SelectListItem> GetAnimeStudios()
        {
            StudioDao studioDao = new StudioDao();
            List<Studio> studios = studioDao.FindAll(null, 0);
            
            List<SelectListItem> l = new List<SelectListItem>();
            
            foreach (var item in studios)
            {
                Debug.WriteLine(item.name);
                l.Add(new SelectListItem {Value = item.name, Text = item.name});
            }

            
            return l;
        }

        public static string GetFavInfo(int id, string userId, string typeId)
        {
            FavouriteDao fd = new FavouriteDao();
            if (!fd.IsExist(id, userId, typeId))
                return "images/heart0.svg";
            return "images/heart1.svg";
        }

        public static List<SelectListItem> GetUserLists(string listName, string userId, string mediaType)
        {
            // Zjistit, jestli je v listu
            MyListDao mld = new MyListDao();
            var t = mld.FindAllById("l.TYPE = '" + mediaType + "'");
            Debug.WriteLine("itemhere");
            // List<string> listNames = new List<string>();
            // List<int> listIds = new List<int>();
            List<SelectListItem> l = new List<SelectListItem>();

            if (listName != null)
            {
                foreach (var item in t)
                {
                    if (item.name == listName)
                        l.Add(new SelectListItem {Value = item.listId.ToString(), Text = listName});
                }

                foreach (var item in t)
                {
                    if (item.name != listName)
                    {
                        Debug.WriteLine(item.name);
                        // listNames.Add(item.name);
                        // listIds.Add(item.listId);
                        l.Add(new SelectListItem {Value = item.listId.ToString(), Text = item.name});
                    }
                }

                return l;
            }
            else
            {
                // MyListDao mld = new MyListDao();
                // var t = mld.FindAllById("l.TYPE = '" + mediaType + "'");
                // Debug.WriteLine("itemhere");
                // List<string> listNames = new List<string>();
                // List<int> listIds = new List<int>();
                // List<SelectListItem> l = new List<SelectListItem>();
                l.Add(new SelectListItem {Value = 0.ToString(), Text = "Přidat do listu"});
                foreach (var item in t)
                {
                    Debug.WriteLine(item.name);
                    // listNames.Add(item.name);
                    // listIds.Add(item.listId);
                    l.Add(new SelectListItem {Value = item.listId.ToString(), Text = item.name});
                }

                return l;
            }
        }

        public static List<SelectListItem> GetLists(string listName, string mediaType)
        {
            // Zjistit, jestli je v listu
            MyListDao mld = new MyListDao();
            var t = mld.FindAllById("l.TYPE = '" + mediaType + "'");
            Debug.WriteLine("itemhere");
            // List<string> listNames = new List<string>();
            // List<int> listIds = new List<int>();
            List<SelectListItem> l = new List<SelectListItem>();
            if (listName != null)
            {
                foreach (var item in t)
                {
                    if (item.name == listName)
                        l.Add(new SelectListItem {Value = item.listId.ToString(), Text = listName});
                }

                foreach (var item in t)
                {
                    if (item.name != listName)
                    {
                        Debug.WriteLine(item.name);
                        l.Add(new SelectListItem {Value = item.listId.ToString(), Text = item.name});
                    }
                }

                return l;
            }
            else
            {
                l.Add(new SelectListItem {Value = null, Text = "Všechny listy"});
                foreach (var item in t)
                {
                    Debug.WriteLine(item.name);
                    // listNames.Add(item.name);
                    // listIds.Add(item.listId);
                    l.Add(new SelectListItem {Value = item.listId.ToString(), Text = item.name});
                }

                return l;
            }
        }

        private string GeneratFileName()
        {
            DateTime now = DateTime.Now;
            string fileName = now.ToString().Replace(" ", "-").Replace(":", "-");
            return fileName;
        }

        public static async Task<string> UploadImg(string path, string folder, IFormFileCollection filesData)
        {
            string finalPath = "";
            var files = filesData;
            foreach (var Image in files)
            {
                if (Image != null && Image.Length > 0)
                {
                    var file = Image;
                    //There is an error here
                    var uploads = path;
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            Debug.WriteLine("file");
                            Debug.WriteLine(fileName);
                            finalPath = "uploads/" + folder + "/" + fileName;
                            return "uploads/" + folder + "/" + fileName;
                        }
                    }
                }
            }

            return finalPath;
        }

    }
}