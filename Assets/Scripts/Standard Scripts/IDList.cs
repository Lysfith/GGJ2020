public class IDList
{
    public int id;
    public IDList next;

    public IDList(short id, IDList next)
    {
        this.id = id;
        this.next = next;
    }

    /// <summary>
    /// Renvoie true si l'id donné est dans la liste
    /// </summary>
    public bool IdIsInList(int id)
    {
        IDList temp = this;
        while (temp != null)
        {
            if (temp.id == id)
                return true;
            temp = temp.next;
        }

        return false;
    }
    /// <summary>
    /// Renvoie une liste sans le 1er element de l'id donné
    /// </summary>
    public IDList WithRemovedFromList(int id)
    {
        if (this.id == id)
            return this.next;

        this.next = this.next.WithRemovedFromList(id);
        return this;
    }

}
