using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blackmaw.Dal.Core;
using Blackmaw.Dal.DbContext;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace blackmaw.api.Core
{
    [Route("api/[controller]")]
    public abstract class ControllerBase<TEntity, TModel> : Controller
        where TEntity : EntityBase
        where TModel : ModelBase
    {
        protected readonly BmDbContext Context;
        protected readonly ILogger Logger;

        protected ControllerBase(BmDbContext context, ILogger logger)
        {
            this.Context = context;
            this.Logger = logger;
        }

        [HttpGet]
        public virtual async Task<IEnumerable<TModel>> GetAll()
        {
            return await this.Context.Set<TEntity>().ProjectTo<TModel>().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var record = await this.Context.Set<TEntity>().FindAsync(id);
            if (record == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<TModel>(record);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var record = Mapper.Map<TEntity>(model);
            this.Context.Set<TEntity>().Add(record);
            await this.Context.SaveChangesAsync();

            return CreatedAtRoute(new { id = record.Id }, record);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TModel model)
        {
            if (model == null || model.Id != id)
            {
                return BadRequest();
            }

            var record = this.Context.Set<TEntity>().Find(id);
            if (record == null)
            {
                return NotFound();
            }

            Mapper.Map(model, record);
            
            this.Context.Set<TEntity>().Update(record);
            await this.Context.SaveChangesAsync();
            return AcceptedAtRoute(new { id = model.Id }, model);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody]JsonPatchDocument<TModel> patch)
        {
            if (!this.ModelState.IsValid)
            {
                return new BadRequestObjectResult(this.ModelState);
            }
            
            var record = await this.Context.Set<TEntity>().FindAsync(id);
            var mappedRecord = Mapper.Map<TModel>(record);
            patch.ApplyTo(mappedRecord, this.ModelState);
            
            Mapper.Map(mappedRecord, record);

            return AcceptedAtRoute(new { id = record.Id }, record);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var todo = this.Context.Set<TEntity>().Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            this.Context.Set<TEntity>().Remove(todo);
            await this.Context.SaveChangesAsync();
            return NoContent();
        }
    }

    public abstract class ControllerBase<TEntity, TModel, TListModel> : ControllerBase<TEntity, TModel>
        where TEntity : EntityBase
        where TModel : ModelBase
        where TListModel : ModelBase
    {
        protected ControllerBase(BmDbContext context, ILogger logger) : base(context, logger)
        {
        }

        [HttpGet]
        public new async Task<IEnumerable<TListModel>> GetAll()
        {
            return await this.Context.Set<TEntity>().ProjectTo<TListModel>().ToListAsync();
        }
    }
}