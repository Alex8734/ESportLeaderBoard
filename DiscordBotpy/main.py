import discord
from discord.ext import commands
from discord_slash import SlashCommand,SlashContext
import json
from cogs import LeaderBoard
import requests
from discord_slash.utils.manage_commands import create_choice,create_option
import os

invite = r'https://discord.com/api/oauth2/authorize?client_id=1186651122375606303&permissions=8&scope=bot+applications.commands'

intents = discord.Intents.default()
intents.members = True

bot = commands.Bot(command_prefix= "!", intents=intents)
slash = SlashCommand(bot, sync_commands=True, sync_on_cog_reload=True)

@bot.event
async def on_ready():
    LeaderBoard.GetGames()
    print(f"{bot.user} has successfully logged in...")

@bot.event
async def on_member_join(member):
    requestPlayer = {"name":member.display_name,"profilePicture":str(member.avatar_url)}
    out = requests.post(f"http://localhost:5001/Player", json=requestPlayer,  headers={'Content-Type': 'application/json'}, )
    print(f"```css\n{out.status_code}-{out.reason}```")

@bot.event
async def on_member_remove(member):
    out = requests.delete(f"http://localhost:5001/Player/{member.display_name}")
    print(f"```css\n{out.status_code}-{out.reason}```")


if __name__ == '__main__':
    for ext in os.listdir('./cogs/'):
        if (ext.endswith('.py')):
            bot.load_extension('cogs.'+ext.rstrip('.py'))

bot.run('MTE4NjY1MTEyMjM3NTYwNjMwMw.GzTMJ0.2sYLvdkgtqQtO6Ar8zz2VfVe0mmqs4Pj0oYiwY')
