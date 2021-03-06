﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blackmaw.Dal.Core;
using Blackmaw.Dal.DbContext;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blackmaw.Api.Core
{
    [ApiController]
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
        public virtual async Task<ActionResult<IEnumerable<TModel>>> GetAll()
        {
            return await this.Context.Set<TEntity>().ProjectTo<TModel>().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TModel>> GetById(string id)
        {
            var record = await this.Context.Set<TEntity>().FindAsync(id);
            if (record == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<TModel>(record);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<CreatedAtRouteResult>> Create([FromBody] TModel model)
        {
            var record = Mapper.Map<TEntity>(model);
            this.Context.Set<TEntity>().Add(record);
            await this.Context.SaveChangesAsync();

            return CreatedAtRoute(new { id = record.Id }, record);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AcceptedAtRouteResult>> Update(string id, [FromBody] TModel model)
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
        public async Task<ActionResult<AcceptedAtRouteResult>> Patch(string id, [FromBody]JsonPatchDocument<TModel> patch)
        {
            var record = await this.Context.Set<TEntity>().FindAsync(id);
            var mappedRecord = Mapper.Map<TModel>(record);
            patch.ApplyTo(mappedRecord, this.ModelState);
            
            Mapper.Map(mappedRecord, record);

            return AcceptedAtRoute(new { id = record.Id }, record);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<NoContentResult>> Delete(string id)
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

    public abstract class ControllerBase<TEntity, TModel, TListModel> : Controller
        where TEntity : EntityBase
        where TModel : ModelBase
        where TListModel : ModelBase
    {
        protected readonly BmDbContext Context;
        protected readonly ILogger Logger;

        protected ControllerBase(BmDbContext context, ILogger logger)
        {
            this.Context = context;
            this.Logger = logger;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TListModel>>> GetAll()
        {
            return await this.Context.Set<TEntity>().ProjectTo<TListModel>().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TModel>> GetById(string id)
        {
            var record = await this.Context.Set<TEntity>().FindAsync(id);
            if (record == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<TModel>(record);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<CreatedAtRouteResult>> Create([FromBody] TModel model)
        {
            var record = Mapper.Map<TEntity>(model);
            this.Context.Set<TEntity>().Add(record);
            await this.Context.SaveChangesAsync();

            return CreatedAtRoute(new { id = record.Id }, record);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AcceptedAtRouteResult>> Update(string id, [FromBody] TModel model)
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
        public async Task<ActionResult<AcceptedAtRouteResult>> Patch(string id, [FromBody]JsonPatchDocument<TModel> patch)
        {
            var record = await this.Context.Set<TEntity>().FindAsync(id);
            var mappedRecord = Mapper.Map<TModel>(record);
            patch.ApplyTo(mappedRecord, this.ModelState);

            Mapper.Map(mappedRecord, record);

            return AcceptedAtRoute(new { id = record.Id }, record);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<NoContentResult>> Delete(string id)
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
}