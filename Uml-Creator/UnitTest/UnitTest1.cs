using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uml_Creator.UndoRedo;
using Uml_Creator.UndoRedo.Commands;
using Uml_Creator.ViewModel;
 

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public ObservableCollection<FigureViewModel> FiguresViewModels { get; private set; }
        public ObservableCollection<Object> MethodCollection { get; set; }


        [TestMethod]
        public void AddClassTest()
        {
            // setup
            FiguresViewModels = new ObservableCollection<FigureViewModel>();
            // create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();
            // depend
            var fvm = new FigureViewModel();
            var box = new AddBoxCommand(FiguresViewModels, fvm);

            // act
            undoRedoController.DoExecute(box);
            // assert
            Assert.IsTrue(1 == FiguresViewModels.Count);

            // act
            undoRedoController.DoExecute(box);
            // assert
            Assert.IsTrue(2 == FiguresViewModels.Count);
        }

        [TestMethod]
        public void RemoveClassTest()
        {
            // setup
            FiguresViewModels = new ObservableCollection<FigureViewModel>();
           
            // create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();

            // depend
            var fvm = new FigureViewModel();
            var box = new DeleteFigureCommand(FiguresViewModels, fvm);
            var addbox = new AddBoxCommand(FiguresViewModels, fvm);

            // act
            undoRedoController.DoExecute(addbox);

            // act
            undoRedoController.DoExecute(box);
            // assert
            Assert.IsTrue(0 == FiguresViewModels.Count);
            
        }

        [TestMethod]
        public void ClassUndoTest()
        {
            // setup
            FiguresViewModels = new ObservableCollection<FigureViewModel>();
            // create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();
            // depend
            var fvm = new FigureViewModel();
            var box = new AddBoxCommand(FiguresViewModels, fvm);

            // act
            undoRedoController.DoExecute(box);
            // assert
            Assert.IsTrue(1 == undoRedoController.UndoStackSize);

            // act
            undoRedoController.Undo();
            Assert.IsTrue(0 == undoRedoController.UndoStackSize);
        }

        [TestMethod]
        public void ClassRedoTest()
        {
            // setup
            FiguresViewModels = new ObservableCollection<FigureViewModel>();
            // create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();
            // depend
            var fvm = new FigureViewModel();
            var box = new AddBoxCommand(FiguresViewModels, fvm);

            // act
            undoRedoController.DoExecute(box);
            // assert
            Assert.IsTrue(1 == undoRedoController.UndoStackSize);
            Assert.IsTrue(0 == undoRedoController.RedoStackSize);

            // act
            undoRedoController.Undo();
            Assert.IsTrue(0 == undoRedoController.UndoStackSize);
            Assert.IsTrue(1 == undoRedoController.RedoStackSize);

            undoRedoController.Redo();
            Assert.IsTrue(1 == undoRedoController.UndoStackSize);
            Assert.IsTrue(0 == undoRedoController.RedoStackSize);
        }

        [TestMethod]
        public void AddMethodTest()
        {
            //create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();

            //depend
            var fvm = new FigureViewModel();
            var cc = new ClassContent("method");
            var method = new AddMethod(fvm, cc);

            // act
            undoRedoController.DoExecute(method);

            // assert
            Assert.IsTrue(1 == fvm.MethodCollection.Count);
        }

        [TestMethod]
        public void RemoveMethodTest()
        {
            //create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();

            //depend
            var fvm = new FigureViewModel();
            var cc = new ClassContent("method");
            var addmethod = new AddMethod(fvm, cc);
            var removemethod = new DeleteMethodCommand(fvm, cc);

            // act
            undoRedoController.DoExecute(addmethod);
            undoRedoController.DoExecute(addmethod);

            // assert
            Assert.IsTrue(2 == fvm.MethodCollection.Count);

            // act
            undoRedoController.DoExecute(removemethod);

            // assert
            Assert.IsTrue(1 == fvm.MethodCollection.Count);
        }

        [TestMethod]
        public void MethodUndoTest()
        {
            // setup
            FiguresViewModels = new ObservableCollection<FigureViewModel>();
            // create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();

            //depend
            var fvm = new FigureViewModel();
            var cc = new ClassContent("method");
            var method = new AddMethod(fvm, cc);

            // act
            undoRedoController.DoExecute(method);
            // assert
            Assert.IsTrue(1 == undoRedoController.UndoStackSize);

            // act
            undoRedoController.Undo();

            //assert
            Assert.IsTrue(0 == undoRedoController.UndoStackSize);
        }

        [TestMethod]
        public void MethodRedoTest()
        {
            // setup
            FiguresViewModels = new ObservableCollection<FigureViewModel>();
            // create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();

            //depend
            var fvm = new FigureViewModel();
            var cc = new ClassContent("method");
            var method = new AddMethod(fvm, cc);

            // act
            undoRedoController.DoExecute(method);
           
            // assert
            Assert.IsTrue(1 == undoRedoController.UndoStackSize);
            Assert.IsTrue(0 == undoRedoController.RedoStackSize);

            // act
            undoRedoController.Undo();

            //assert
            Assert.IsTrue(0 == undoRedoController.UndoStackSize);
            Assert.IsTrue(1 == undoRedoController.RedoStackSize);

            //act
            undoRedoController.Redo();

            //assert
            Assert.IsTrue(1 == undoRedoController.UndoStackSize);
            Assert.IsTrue(0 == undoRedoController.RedoStackSize);
        }

        [TestMethod]
        public void AddAttributeTest()
        {
            //create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();

            //depend
            var fvm = new FigureViewModel();
            var cc = new ClassContent("method");
            var attribute = new AddAttributeCommand(fvm, cc);

            // act
            undoRedoController.DoExecute(attribute);

            // assert
            Assert.IsTrue(1 == fvm.AttributeCollection.Count);
        }

        [TestMethod]
        public void RemoveAttributeTest()
        {
            //create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();

            //depend
            var fvm = new FigureViewModel();
            var cc = new ClassContent("method");
            var addattribute = new AddAttributeCommand(fvm, cc);
            var removeattribute = new DeleteAttribute(fvm, cc);

            // act
            undoRedoController.DoExecute(addattribute);
            undoRedoController.DoExecute(addattribute);

            // assert
            Assert.IsTrue(2 == fvm.AttributeCollection.Count);

            // act
            undoRedoController.DoExecute(removeattribute);

            // assert
            Assert.IsTrue(1 == fvm.AttributeCollection.Count);
        }

        [TestMethod]
        public void AttributeUndoTest()
        {
            // setup
            FiguresViewModels = new ObservableCollection<FigureViewModel>();
            // create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();

            //depend
            var fvm = new FigureViewModel();
            var cc = new ClassContent("method");
            var method = new AddAttributeCommand(fvm, cc);

            // act
            undoRedoController.DoExecute(method);

            // assert
            Assert.IsTrue(1 == undoRedoController.UndoStackSize);

            // act
            undoRedoController.Undo();

            // assert
            Assert.IsTrue(0 == undoRedoController.UndoStackSize);
        }

        [TestMethod]
        public void AttributeRedoTest()
        {
            // setup
            FiguresViewModels = new ObservableCollection<FigureViewModel>();
            // create
            UndoRedoController undoRedoController = UndoRedoController.Instance;
            undoRedoController.reset();

            //depend
            var fvm = new FigureViewModel();
            var cc = new ClassContent("method");
            var method = new AddAttributeCommand(fvm, cc);

            // act
            undoRedoController.DoExecute(method);

            // assert
            Assert.IsTrue(1 == undoRedoController.UndoStackSize);
            Assert.IsTrue(0 == undoRedoController.RedoStackSize);

            // act
            undoRedoController.Undo();
            
            //assert
            Assert.IsTrue(0 == undoRedoController.UndoStackSize);
            Assert.IsTrue(1 == undoRedoController.RedoStackSize);

            // act
            undoRedoController.Redo();

            //assert
            Assert.IsTrue(1 == undoRedoController.UndoStackSize);
            Assert.IsTrue(0 == undoRedoController.RedoStackSize);
        }

    }
}
