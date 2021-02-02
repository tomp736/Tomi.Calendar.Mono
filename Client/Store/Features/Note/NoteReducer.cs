using Fluxor;
using System.Collections.Generic;
using System.Linq;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared.Dtos.Note;

namespace Tomi.Calendar.Mono.Client.Store.Features.Note
{
    public static class NoteReducer
    {
        #region Create

        [ReducerMethod]
        public static CalendarState ReduceCreateNoteAction(CalendarState state, CreateNoteAction _) =>
            state with
            {
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState ReduceCreateNoteSuccessAction(CalendarState state, CreateNoteSuccessAction action)
        {
            // Grab a reference to the current Note list, or initialize one if we do not currently have any loaded
            var currentNotes = state.Notes is null ?
                new List<NoteDto>() :
                state.Notes.ToList();
            // Add the newly created Note to our list and sort by ID
            currentNotes.Add(action.Note);
            currentNotes = currentNotes
                .OrderBy(t => t.Id)
                .ToList();

            return state with
            {

                CurrentErrorMessage = null,
                Notes = currentNotes
            };
        }

        [ReducerMethod]
        public static CalendarState ReduceCreateNoteFailureAction(CalendarState state, CreateNoteFailureAction action) =>
            state with
            {
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region Load

        [ReducerMethod]
        public static CalendarState ReduceLoadNotesAction(CalendarState state, LoadNotesAction _) =>
            state with
            {
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState ReduceLoadNotesSuccessAction(CalendarState state, LoadNotesSuccessAction action) =>
            state with
            {
                CurrentErrorMessage = null,
                Notes = action.Notes
            };

        [ReducerMethod]
        public static CalendarState ReduceLoadNotesFailureAction(CalendarState state, LoadNotesFailureAction action) =>
            state with
            {
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region LoadDetail

        [ReducerMethod]
        public static CalendarState ReduceLoadNoteDetailAction(CalendarState state, LoadNoteDetailAction _) =>
            state with
            {
            };

        [ReducerMethod]
        public static CalendarState ReduceLoadNoteDetailSuccessAction(CalendarState state, LoadNoteDetailSuccessAction action) =>
            state with
            {
                CurrentNote = action.Note,
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState ReduceLoadNoteDetailFailureAction(CalendarState state, LoadNoteDetailFailureAction action) =>
            state with
            {
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region Update

        [ReducerMethod]
        public static CalendarState ReduceUpdateNoteAction(CalendarState state, UpdateNoteAction _) =>
            state with
            {
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState ReduceUpdateNoteSuccessAction(CalendarState state, UpdateNoteSuccessAction action)
        {
            // If the current Notes list is null, set the state with a new list containing the updated Note
            if (state.Notes is null)
            {
                return state with
                {

                    CurrentErrorMessage = null,
                    Notes = new List<NoteDto> { action.Note }
                };
            }

            // Rather than mutating in place, let's construct a new list and add our updated item
            var updatedList = state.Notes
                .Where(t => t.Id != action.Note.Id)
                .ToList();

            // Add the Note and sort the list
            updatedList.Add(action.Note);
            updatedList = updatedList
                .OrderBy(t => t.Id)
                .ToList();

            return state with
            {

                CurrentErrorMessage = null,
                Notes = updatedList
            };
        }

        [ReducerMethod]
        public static CalendarState ReduceUpdateNoteFailureAction(CalendarState state, UpdateNoteFailureAction action) =>
            state with
            {
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region Delete

        [ReducerMethod]
        public static CalendarState ReduceDeleteNoteAction(CalendarState state, DeleteNoteAction _) =>
            state with
            {
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState ReduceDeleteNoteSuccessAction(CalendarState state, DeleteNoteSuccessAction action)
        {
            // Return the default state if no list of Notes is found
            if (state.Notes is null)
            {
                return state with
                {

                    CurrentErrorMessage = null
                };
            }

            // Create a new list with all Note items excluding the Note with the deleted ID
            var updatedNotes = state.Notes.Where(t => t.Id != action.Id);
            return state with
            {
                CurrentErrorMessage = null,
                Notes = updatedNotes
            };
        }
        [ReducerMethod]
        public static CalendarState ReduceDeleteNoteFailureAction(CalendarState state, DeleteNoteFailureAction action) =>
            state with
            {
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion
    }
}
