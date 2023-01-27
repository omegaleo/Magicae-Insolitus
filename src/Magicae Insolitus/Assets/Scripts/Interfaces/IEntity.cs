using System;

public interface IEntity
{
    public Guid GUID();
    
    public void DoDamage(float damage);
}