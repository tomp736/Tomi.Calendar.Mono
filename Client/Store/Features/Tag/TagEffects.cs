using Fluxor;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Client.Store.Features.Tag
{
    public class TagEffects
    {
        private readonly CalendarDataService _calendarDataService;
        private readonly ILogger<TagEffects> _logger;
        private readonly IState<CalendarState> _state;

        public TagEffects(CalendarDataService calendarDataService, ILogger<TagEffects> logger, IState<CalendarState> state)
        {
            _calendarDataService = calendarDataService;
            _logger = logger;
            _state = state;
        }

        [EffectMethod]
        public async Task HandleAsync(LoadTagsAction action, IDispatcher dispatcher)
        {
            try
            {
                var notes = await _calendarDataService.GetTags(new GetTagsRequest());
                dispatcher.Dispatch(new LoadTagsSuccessAction(notes.Tags));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadTagsFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(LoadTagDetailAction action, IDispatcher dispatcher)
        {
            try
            {
                GetTagsResponse calendarItemsResponse = await _calendarDataService.GetTags(new GetTagsRequest()
                {
                    TagIds = new Guid[] { action.Id }
                });
                TagDto calendarItemDto = calendarItemsResponse.Tags.FirstOrDefault();
                if (calendarItemDto != null)
                {
                    dispatcher.Dispatch(new LoadTagDetailSuccessAction(calendarItemDto));
                }
                else
                {
                    dispatcher.Dispatch(new LoadTagDetailFailureAction("Resource does not exist"));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadTagDetailFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(NewTagAction action, IDispatcher dispatcher)
        {
            try
            {
                GetTagsResponse calendarItemsResponse = await _calendarDataService.GetTags(new GetTagsRequest()
                {
                    TagIds = new Guid[] { action.Id }
                });
                if (calendarItemsResponse.Tags != null &&
                    calendarItemsResponse.Tags.Any())
                {
                    dispatcher.Dispatch(new NewTagFailureAction($"Resource already exists for {action.Id}"));
                }
                else
                {
                    dispatcher.Dispatch(new NewTagSuccessAction(action.Id));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new NewTagFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(UpdateTagAction action, IDispatcher dispatcher)
        {
            try
            {
                TagDto tagDto = new TagDto();
                tagDto.Id = action.Id;
                tagDto.Name = action.TagDto.Name;
                tagDto.Description = action.TagDto.Description;
                tagDto.Color = action.TagDto.Color;

                SaveTagsResponse calendarItemsResponse = await _calendarDataService.SaveTags(new SaveTagsRequest()
                {
                    Tags = new TagDto[] { tagDto }
                });
                if (calendarItemsResponse.Tags.Any())
                {
                    dispatcher.Dispatch(new UpdateTagSuccessAction(calendarItemsResponse.Tags.FirstOrDefault()));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new UpdateTagFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(CreateTagAction action, IDispatcher dispatcher)
        {
            try
            {
                TagDto tagDto = new TagDto();
                tagDto.Name = action.Tag.Name;
                tagDto.Description = action.Tag.Description;

                SaveTagsResponse calendarItemsResponse = await _calendarDataService.SaveTags(new SaveTagsRequest()
                {
                    Tags = new TagDto[] { tagDto }
                });
                if (calendarItemsResponse.Tags.Any())
                {
                    dispatcher.Dispatch(new UpdateTagSuccessAction(calendarItemsResponse.Tags.FirstOrDefault()));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new UpdateTagFailureAction(e.Message));
            }
        }


        [EffectMethod]
        public async Task HandleAsync(DeleteTagAction action, IDispatcher dispatcher)
        {
            try
            {
                DeleteTagsResponse calendarItemsResponse = await _calendarDataService.DeleteTags(new DeleteTagsRequest()
                {
                    TagIds = new Guid[] { action.Id }
                });
                dispatcher.Dispatch(new DeleteTagSuccessAction(action.Id));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new DeleteTagFailureAction(e.Message));
            }
        }
    }
}
