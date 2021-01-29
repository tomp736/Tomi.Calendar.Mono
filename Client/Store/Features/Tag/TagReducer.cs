using Fluxor;
using System.Collections.Generic;
using System.Linq;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;

namespace Tomi.Calendar.Mono.Client.Store.Features.Tag
{
    public static class TagReducer
    {
        #region Create

        [ReducerMethod]
        public static CalendarState ReduceCreateTagAction(CalendarState state, CreateTagAction _) =>
            state with
            {
                
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState ReduceCreateTagSuccessAction(CalendarState state, CreateTagSuccessAction action)
        {
            // Grab a reference to the current Tag list, or initialize one if we do not currently have any loaded
            var currentTags = state.Tags is null ?
                new List<TagDto>() :
                state.Tags.ToList();
            // Add the newly created Tag to our list and sort by ID
            currentTags.Add(action.Tag);
            currentTags = currentTags
                .OrderBy(t => t.Id)
                .ToList();

            return state with
            {
                
                CurrentErrorMessage = null,
                Tags = currentTags
            };
        }

        [ReducerMethod]
        public static CalendarState ReduceCreateTagFailureAction(CalendarState state, CreateTagFailureAction action) =>
            state with
            {
                
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region Load

        [ReducerMethod]
        public static CalendarState ReduceLoadTagsAction(CalendarState state, LoadTagsAction _) =>
            state with
            {
                
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState ReduceLoadTagsSuccessAction(CalendarState state, LoadTagsSuccessAction action) =>
            state with
            {
                
                CurrentErrorMessage = null,
                Tags = action.Tags
            };

        [ReducerMethod]
        public static CalendarState ReduceLoadTagsFailureAction(CalendarState state, LoadTagsFailureAction action) =>
            state with
            {
                
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region LoadDetail

        [ReducerMethod]
        public static CalendarState ReduceLoadTagDetailAction(CalendarState state, LoadTagDetailAction _) =>
            state with
            {
                
            };

        [ReducerMethod]
        public static CalendarState ReduceLoadTagDetailSuccessAction(CalendarState state, LoadTagDetailSuccessAction action) =>
            state with
            {
                
                CurrentTag = action.Tag,
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState ReduceLoadTagDetailFailureAction(CalendarState state, LoadTagDetailFailureAction action) =>
            state with
            {
                
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region Update

        [ReducerMethod]
        public static CalendarState ReduceUpdateTagAction(CalendarState state, UpdateTagAction _) =>
            state with
            {
                
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState ReduceUpdateTagSuccessAction(CalendarState state, UpdateTagSuccessAction action)
        {
            // If the current Tags list is null, set the state with a new list containing the updated Tag
            if (state.Tags is null)
            {
                return state with
                {
                    
                    CurrentErrorMessage = null,
                    Tags = new List<TagDto> { action.Tag }
                };
            }

            // Rather than mutating in place, let's construct a new list and add our updated item
            var updatedList = state.Tags
                .Where(t => t.Id != action.Tag.Id)
                .ToList();

            // Add the Tag and sort the list
            updatedList.Add(action.Tag);
            updatedList = updatedList
                .OrderBy(t => t.Id)
                .ToList();

            return state with
            {
                
                CurrentErrorMessage = null,
                Tags = updatedList
            };
        }

        [ReducerMethod]
        public static CalendarState ReduceUpdateTagFailureAction(CalendarState state, UpdateTagFailureAction action) =>
            state with
            {
                
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion

        #region Delete

        [ReducerMethod]
        public static CalendarState ReduceDeleteTagAction(CalendarState state, DeleteTagAction _) =>
            state with
            {
                
                CurrentErrorMessage = null
            };

        [ReducerMethod]
        public static CalendarState ReduceDeleteTagSuccessAction(CalendarState state, DeleteTagSuccessAction action)
        {
            // Return the default state if no list of Tags is found
            if (state.Tags is null)
            {
                return state with
                {
                    
                    CurrentErrorMessage = null
                };
            }

            // Create a new list with all Tag items excluding the Tag with the deleted ID
            var updatedTags = state.Tags.Where(t => t.Id != action.Id);
            return state with
            {
                
                CurrentErrorMessage = null,
                Tags = updatedTags
            };
        }
        [ReducerMethod]
        public static CalendarState ReduceDeleteTagFailureAction(CalendarState state, DeleteTagFailureAction action) =>
            state with
            {
                
                CurrentErrorMessage = action.ErrorMessage
            };

        #endregion
    }
}
