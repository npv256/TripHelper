using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interfaces;
using DLL.Entities;
using WEB.Models;

namespace WEB.Controllers
{
    public class CommentController : Controller
    {

        private readonly IService<User> _userService;
        private readonly IService<Track> _trackService;
        private readonly IService<Place> _placeService;
        private readonly IService<CommentPlace> _commentPlaceService;
        private readonly IService<CommentTrack> _commentTrackService;

        public CommentController(IService<User> userService, IService<Place> placeService, IService<Track> trackService,
            IService<CommentPlace> commentPlaceService, IService<CommentTrack> commentTrackService)
        {
            _userService = userService;
            _placeService = placeService;
            _trackService = trackService;
            _commentPlaceService = commentPlaceService;
            _commentTrackService = commentTrackService;
        }

        public ActionResult CheckCommentExist(long id, string typeComment)
        {
            if(!User.Identity.IsAuthenticated)
                return Json(3, JsonRequestBehavior.AllowGet);
            var author = _userService.GetItemList().ToList().FirstOrDefault(user => user.Email == User.Identity.Name);
            if (typeComment == "Place")
            {
                var query = _placeService.GetItem(id).Comments.FirstOrDefault(comment=>comment.Author==author);
                if (query != null)
                    return Json(1, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            if (typeComment == "Track")
            {
                if (_trackService.GetItem(id).Comments.FirstOrDefault(comment => comment.Author == author) != null)
                    return Json(1, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComments(long id, string typeComment, int count = 5, int list = 1)
        {
            try
            {
            if (typeComment == "Place")
            {
                List<CommentPlace> comments = _commentPlaceService.GetItemList().Where(comment => comment.PlaceId == id).ToList();
                List<CommentModels> commentModels = new List<CommentModels>();
                foreach (var comment in comments)
                {
                    CommentModels commentModel = new CommentModels();
                    commentModel.Id = comment.Id;
                    commentModel.TargetId = comment.PlaceId;
                    commentModel.Assessment = comment.Assessment;
                    commentModel.Text = comment.Text;
                    commentModel.Rating = comment.Rating;
                    commentModel.Picture = comment.Pictures.FirstOrDefault();
                    commentModel.Author = comment.Author;
                    commentModel.TypeComment = "Place";
                    commentModels.Add(commentModel);
                    if (commentModels.Count >= count * list)
                    {
                        var index = count * list - count;
                        commentModels = commentModels.GetRange(index, count);
                        return PartialView(commentModels);
                    }
                }
                if(commentModels.Count==0)
                    return Json(0, JsonRequestBehavior.AllowGet);
                else
                    return PartialView(commentModels);
                }

            if (typeComment == "Track")
            {
                List<CommentTrack> comments = _commentTrackService.GetItemList().Where(comment => comment.TrackId == id)
                    .ToList();
                List<CommentModels> commentModels = new List<CommentModels>();
                foreach (var comment in comments)
                {
                    CommentModels commentModel = new CommentModels();
                    commentModel.Id = comment.Id;
                    commentModel.TargetId = comment.TrackId;
                    commentModel.Assessment = comment.Assessment;
                    commentModel.Text = comment.Text;
                    commentModel.Rating = comment.Rating;
                    commentModel.Picture = comment.Pictures.FirstOrDefault();
                    commentModel.Author = comment.Author;
                    commentModel.TypeComment = "Track";
                    commentModels.Add(commentModel);
                    if (commentModels.Count >= count * list)
                    {
                        var index = count * list - count;
                        commentModels = commentModels.GetRange(index, count);
                        return PartialView(commentModels);
                    }
                }

                if (commentModels.Count == 0)
                    return Json(0, JsonRequestBehavior.AllowGet);
                else
                    return PartialView(commentModels);
                }
            else
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        public ActionResult Create(long id, string typeComment)
      {
          if (!User.Identity.IsAuthenticated)
                return Json(0, JsonRequestBehavior.AllowGet);
            CommentModels comment = new CommentModels();
            comment.TypeComment = typeComment;
            comment.TargetId = id;
            return PartialView(comment);        
      }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentModels commentModels,
            IEnumerable<HttpPostedFileBase> fileData)
        {
            try
            {
                List<Picture> pictures = new List<Picture>();
                if (ModelState.IsValid)
                {

                    if (fileData != null)
                    {
                            fileData = fileData.Where(f => f != null);
                        foreach (var someFile in fileData)
                        {
                            System.IO.FileInfo file = new System.IO.FileInfo(someFile.FileName);
                            if (file.Extension == ".jpeg" || file.Extension == ".jpg" || file.Extension == ".png" ||
                                file.Extension == ".gif" || file.Extension == ".bmp")
                            {
                                string fname = file.Name.Remove((file.Name.Length - file.Extension.Length));
                                fname = fname + System.DateTime.Now.ToString("_ddMMyyhhmmssms") + file.Extension;
                                someFile.SaveAs(Server.MapPath("../Content/Images/" + fname));
                                Picture pic = new Picture
                                {
                                    Name = fname,
                                    Path = Server.MapPath("../Content/Images/" + fname),
                                };
                                pictures.Add(pic);
                            }
                            else
                            {
                                ModelState.AddModelError("WrongFormatImages",
                                    "Разрешенные только следующие форматы изображений : jpeg, jpg, png, gif, bmp");
                                return PartialView(commentModels);
                            }
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("NoImage", "Не выбрано не одного фото");
                        return PartialView(commentModels);
                    }

                    if (commentModels.TypeComment == "Place")
                    {
                        CommentPlace commetPlace = new CommentPlace();
                        commetPlace.PlaceId = commentModels.TargetId;
                        commetPlace.Assessment = commentModels.Assessment;
                        commetPlace.Pictures = pictures;
                        commetPlace.PlaceComment = _placeService.GetItem(commentModels.TargetId);
                        commetPlace.Author = _userService.GetItemList().First(user => user.Email == User.Identity.Name);
                        commetPlace.AuthorId = _userService.GetItemList().First(user => user.Email == User.Identity.Name).Id;
                        commetPlace.Text = commentModels.Text;
                        _commentPlaceService.Create(commetPlace);
                        _commentPlaceService.Save();
                        var summ = 0;
                        var count = 0;
                        foreach (var comment in _commentPlaceService.GetItemList().Where(place => place.PlaceId == commentModels.TargetId))
                        {
                            summ += comment.Assessment;
                            count++;
                        }
                        var someplace = _placeService.GetItem(commentModels.TargetId);
                        someplace.Rating = summ / count;
                        _placeService.Update(someplace);
                        _placeService.Save();
                        return Json(1);
                    }

                    if (commentModels.TypeComment == "Track")
                    {
                        CommentTrack commentTrack = new CommentTrack();
                        commentTrack.TrackId = commentModels.TargetId;
                        commentTrack.Assessment = commentModels.Assessment;
                        commentTrack.Pictures = pictures;
                        commentTrack.TrackComment = _trackService.GetItem(commentModels.TargetId);
                        commentTrack.Author = _userService.GetItemList().First(user => user.Email == User.Identity.Name);
                        commentTrack.AuthorId = _userService.GetItemList().First(user => user.Email == User.Identity.Name).Id;
                        commentTrack.Text = commentModels.Text;
                        _commentTrackService.Create(commentTrack);
                        _commentTrackService.Save();
                        var summ = 0;
                        var count = 0;
                        foreach (var comment in _commentTrackService.GetItemList()
                            .Where(place => place.TrackId == commentModels.TargetId))
                        {
                            summ += comment.Assessment;
                            count++;
                        }
                        var someTrack = _trackService.GetItem(commentModels.TargetId);
                        someTrack.Rating = summ / count;
                        _trackService.Update(someTrack);
                        _trackService.Save();
                        return Json(1);
                    }
                }
            }
            catch(Exception e)
            {
                return PartialView(commentModels);
            }
            return PartialView(commentModels);

            return PartialView();
        }
    }
}